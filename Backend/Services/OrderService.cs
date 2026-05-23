using Backend.Models;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly AppDbContext _context;

        public OrderService(OrderRepository orderRepository, AppDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            return await _orderRepository.GetOrdersByRestaurantAsync(restaurantId);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders
                .Include(o => o.OrderFoods)
                .FirstOrDefaultAsync(o => o.IdOrder == orderId);
            if (order == null)
            {
                return false;
            }

            string oldStatus = order.Status;
            order.Status = status;
            if (status == "completed")
            {
                if (order.IdDriver == null)
                {
                    // Tự động phân phối tài xế giao hàng
                    var driver = await _context.Drivers.FirstOrDefaultAsync();
                    if (driver != null)
                    {
                        order.IdDriver = driver.IdDriver;
                    }
                }
            }
            else if (status != "delivering")
            {
                order.IdDriver = null; // Hủy gán tài xế khi trả lại hành động trước đó
            }

            order.UpdatedAt = DateTime.Now;
            
            await AdjustFoodDailyQuantityAsync(order, oldStatus, status);
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetDashboardStatsAsync(int restaurantId)
        {
            var today = DateTime.Today;

            // 1. Fetch all orders for this restaurant to calculate in-memory (or query DB)
            var allRestaurantOrders = await _context.Orders
                .Where(o => o.IdRestaurant == restaurantId)
                .ToListAsync();

            var todayOrders = allRestaurantOrders.Where(o => o.CreatedAt.Date == today).ToList();

            int totalOrdersToday = todayOrders.Count;
            decimal revenueToday = todayOrders.Where(o => o.Status != "canceled").Sum(o => o.FinalTotal);
            int preparingOrders = allRestaurantOrders.Count(o => o.Status != "completed" && o.Status != "canceled");

            // 2. Average rating from reviews
            var reviews = await _context.Reviews
                .Where(r => r.IdRestaurant == restaurantId)
                .ToListAsync();
            double avgRating = reviews.Any() ? reviews.Average(r => r.FoodRating) : 5.0;

            // 3. Recent orders (top 5)
            var recentOrders = await _context.Orders
                .Where(o => o.IdRestaurant == restaurantId)
                .Include(o => o.User)
                .Include(o => o.OrderFoods)
                    .ThenInclude(of => of.Food)
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .ToListAsync();

            // 4. Popular food items
            var popularFoods = await _context.Foods
                .Where(f => f.IdRestaurant == restaurantId)
                .OrderByDescending(f => f.CookCount)
                .Take(4)
                .ToListAsync();

            return new
            {
                TotalOrdersToday = totalOrdersToday,
                RevenueToday = revenueToday,
                PreparingOrders = preparingOrders,
                Rating = (decimal)avgRating,
                RecentOrders = recentOrders.Select(o => new
                {
                    o.IdOrder,
                    o.OrderCode,
                    CustomerName = o.User?.FullName ?? "Khách hàng",
                    o.FinalTotal,
                    o.Status,
                    o.CreatedAt,
                    ItemsText = string.Join(", ", o.OrderFoods.Select(of => $"{of.Quantity} {of.Food?.Name}"))
                }),
                PopularItems = popularFoods.Select(f => new
                {
                    f.IdFood,
                    f.Name,
                    f.Price,
                    f.Image,
                    CookCount = f.CookCount ?? 0
                })
            };
        }

        public async Task<object> GetAnalyticsStatsAsync(int restaurantId)
        {
            var now = DateTime.Now;
            var past7Days = Enumerable.Range(0, 7)
                .Select(i => now.Date.AddDays(-i))
                .Reverse()
                .ToList();

            var orders = await _context.Orders
                .Where(o => o.IdRestaurant == restaurantId && o.CreatedAt >= now.Date.AddDays(-6))
                .ToListAsync();

            // Doanh thu theo 7 ngày
            var dailyRevenueList = new List<decimal>();
            var dayLabels = new List<string>();

            // Map DayOfWeek to Vietnamese
            string[] viDays = { "Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };

            foreach (var day in past7Days)
            {
                var dayRevenue = orders
                    .Where(o => o.CreatedAt.Date == day.Date && o.Status != "canceled")
                    .Sum(o => o.FinalTotal);
                dailyRevenueList.Add(dayRevenue);
                dayLabels.Add(viDays[(int)day.DayOfWeek]);
            }

            // Phân phối trạng thái đơn hàng
            var allRestaurantOrders = await _context.Orders
                .Where(o => o.IdRestaurant == restaurantId)
                .ToListAsync();

            var statusCounts = new Dictionary<string, int>
            {
                { "Đã xác nhận", allRestaurantOrders.Count(o => o.Status == "confirmed" || o.Status == "restaurant_accepted") },
                { "Đang chuẩn bị", allRestaurantOrders.Count(o => o.Status == "preparing" || o.Status == "cooked") },
                { "Đã giao", allRestaurantOrders.Count(o => o.Status == "completed" || o.Status == "delivering") }
            };

            return new
            {
                DayLabels = dayLabels,
                RevenueData = dailyRevenueList,
                StatusLabels = statusCounts.Keys.ToList(),
                StatusData = statusCounts.Values.ToList()
            };
        }

        public async Task<bool> UpdateOrderAsync(int orderId, Backend.DTOs.Request.OrderRequest dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderFoods)
                .FirstOrDefaultAsync(o => o.IdOrder == orderId);
            if (order == null)
            {
                return false;
            }

            if (dto.DeliveryAddress != null)
            {
                order.DeliveryAddress = dto.DeliveryAddress;
            }
            if (dto.Note != null)
            {
                order.Note = dto.Note;
            }
            if (dto.Status != null)
            {
                string oldStatus = order.Status;
                order.Status = dto.Status;
                if (dto.Status == "completed")
                {
                    if (order.IdDriver == null)
                    {
                        // Tự động phân phối tài xế giao hàng
                        var driver = await _context.Drivers.FirstOrDefaultAsync();
                        if (driver != null)
                        {
                            order.IdDriver = driver.IdDriver;
                        }
                    }
                }
                else if (dto.Status != "delivering")
                {
                    order.IdDriver = null; // Hủy gán tài xế khi trả lại hành động trước đó
                }
                
                await AdjustFoodDailyQuantityAsync(order, oldStatus, dto.Status);
            }

            order.UpdatedAt = DateTime.Now;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderFoods)
                .Include(o => o.OrderPromotions)
                .Include(o => o.Review)
                    .ThenInclude(r => r.ReviewFoods)
                .Include(o => o.PaymentMethodEntity)
                .Include(o => o.Complaint)
                .FirstOrDefaultAsync(o => o.IdOrder == orderId);

            if (order == null)
            {
                return false;
            }

            // Explicitly remove children if any exist to ensure referential integrity
            if (order.OrderFoods != null && order.OrderFoods.Any())
            {
                _context.OrderFoods.RemoveRange(order.OrderFoods);
            }
            if (order.OrderPromotions != null && order.OrderPromotions.Any())
            {
                _context.OrderPromotions.RemoveRange(order.OrderPromotions);
            }
            if (order.Review != null)
            {
                if (order.Review.ReviewFoods != null && order.Review.ReviewFoods.Any())
                {
                    _context.ReviewFoods.RemoveRange(order.Review.ReviewFoods);
                }
                _context.Reviews.Remove(order.Review);
            }
            if (order.PaymentMethodEntity != null)
            {
                _context.PaymentMethods.Remove(order.PaymentMethodEntity);
            }
            if (order.Complaint != null)
            {
                _context.Complaints.Remove(order.Complaint);
            }

            _context.Orders.Remove(order);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Order> SeedOrderAsync()
        {
            // 1. Fetch or Create Restaurant 1
            var restaurant = await _context.Restaurants.FindAsync(1);
            if (restaurant == null)
            {
                restaurant = new Restaurant
                {
                    IdRestaurant = 1,
                    NameRestaurant = "Bếp Nhà Việt",
                    Description = "Nhà hàng ẩm thực truyền thống Việt Nam",
                    Address = "123 Đường Ba Tháng Hai, Quận 10, TP.HCM",
                    OpenTime = TimeSpan.Parse("08:00:00"),
                    CloseTime = TimeSpan.Parse("22:00:00")
                };
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
            }

            // 2. Fetch or Create Category
            var category = await _context.Categories.FirstOrDefaultAsync();
            if (category == null)
            {
                category = new Category
                {
                    IdCategory = 1,
                    Name = "Món Chính",
                    Icon = "fast-food"
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            // 3. Fetch or Create Foods for Restaurant 1
            var foods = await _context.Foods.Where(f => f.IdRestaurant == 1).ToListAsync();
            if (!foods.Any())
            {
                int maxFoodId = await _context.Foods.AnyAsync() ? await _context.Foods.MaxAsync(f => f.IdFood) : 0;
                var food1 = new Food
                {
                    IdFood = Math.Max(1, maxFoodId + 1),
                    IdRestaurant = 1,
                    IdCategory = category.IdCategory,
                    Name = "Bánh xèo Bếp Nhà Việt",
                    Price = 65000,
                    Discount = 0,
                    Description = "Bánh xèo giòn rụm với nhân tôm thịt",
                    Image = "https://images.unsplash.com/photo-1583032353423-048ba6b11c20?w=500",
                    CookCount = 10,
                    PrepTime = 15
                };

                var food2 = new Food
                {
                    IdFood = food1.IdFood + 1,
                    IdRestaurant = 1,
                    IdCategory = category.IdCategory,
                    Name = "Cá kho tộ đất sét",
                    Price = 85000,
                    Discount = 0,
                    Description = "Cá lóc kho tộ đậm đà hương vị miền Tây",
                    Image = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500",
                    CookCount = 8,
                    PrepTime = 20
                };

                _context.Foods.Add(food1);
                _context.Foods.Add(food2);
                await _context.SaveChangesAsync();
                foods.Add(food1);
                foods.Add(food2);
            }

            // 4. Fetch or Create User (Customer)
            var user = await _context.Users.FirstOrDefaultAsync();
            if (user == null)
            {
                var role = await _context.Roles.FindAsync(2);
                if (role == null)
                {
                    role = new Role
                    {
                        IdRole = 2,
                        RoleName = "customer"
                    };
                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                }

                user = new User
                {
                    IdUser = 1,
                    IdRole = role.IdRole,
                    Username = "khachhangtest",
                    Password = "password123",
                    FullName = "Nguyễn Văn An",
                    Phone = "0987654321",
                    Email = "customer@example.com",
                    Address = "456 Lê Duẩn, Quận 1, TP.HCM",
                    Status = "active",
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // 5. Generate Safe Unique IdOrder
            int nextOrderId = await _context.Orders.AnyAsync() ? await _context.Orders.MaxAsync(o => o.IdOrder) + 1 : 1;

            // 6. Create Order
            var order = new Order
            {
                IdOrder = nextOrderId,
                IdUser = user.IdUser,
                IdRestaurant = 1,
                OrderCode = "DH-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                DeliveryAddress = "789 Điện Biên Phủ, Phường 25, Quận Bình Thạnh, TP.HCM",
                PaymentMethod = "COD",
                PaymentStatus = "pending",
                Status = "pending",
                Note = "Giao hàng giờ hành chính, gọi trước khi đến giúp mình nhé!",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FoodAmount = 0,
                ShippingFee = 15000,
                Discount = 0,
                FinalTotal = 0
            };

            // 7. Add OrderFoods with Unique IDs
            int nextOrderFoodId = await _context.OrderFoods.AnyAsync() ? await _context.OrderFoods.MaxAsync(of => of.IdOrderFood) + 1 : 1;
            var orderFoods = new List<OrderFood>();
            decimal totalFoodAmount = 0;

            var selectedFoods = foods.Take(2).ToList();
            int foodIndex = 0;
            foreach (var food in selectedFoods)
            {
                int qty = foodIndex == 0 ? 2 : 1;
                decimal unitPrice = food.Price;
                decimal totalPrice = unitPrice * qty;
                totalFoodAmount += totalPrice;

                orderFoods.Add(new OrderFood
                {
                    IdOrderFood = nextOrderFoodId++,
                    IdOrder = nextOrderId,
                    IdFood = food.IdFood,
                    Quantity = qty,
                    UnitPrice = unitPrice,
                    TotalPrice = totalPrice,
                    Note = foodIndex == 0 ? "Nhiều rau, không cay" : "Ít hành"
                });
                foodIndex++;
            }

            order.FoodAmount = totalFoodAmount;
            order.FinalTotal = totalFoodAmount + order.ShippingFee - order.Discount;
            order.OrderFoods = orderFoods;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        private async Task AdjustFoodDailyQuantityAsync(Order order, string oldStatus, string newStatus)
        {
            if (newStatus == "delivering" && oldStatus != "delivering")
            {
                if (order.OrderFoods != null)
                {
                    foreach (var of in order.OrderFoods)
                    {
                        var food = await _context.Foods.FindAsync(of.IdFood);
                        if (food != null)
                        {
                            food.DailyQuantity = Math.Max(0, food.DailyQuantity - of.Quantity);
                            _context.Foods.Update(food);
                        }
                    }
                }
            }
            else if ((newStatus == "canceled" || newStatus == "cancelled") && (oldStatus == "delivering" || oldStatus == "completed"))
            {
                if (order.OrderFoods != null)
                {
                    foreach (var of in order.OrderFoods)
                    {
                        var food = await _context.Foods.FindAsync(of.IdFood);
                        if (food != null)
                        {
                            food.DailyQuantity += of.Quantity;
                            _context.Foods.Update(food);
                        }
                    }
                }
            }
        }

        public async Task<List<object>> GetRestaurantReviewsAsync(int restaurantId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.IdRestaurant == restaurantId)
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.ReviewFoods)
                    .ThenInclude(rf => rf.Food)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reviews.Select(r => (object)new
            {
                r.IdReview,
                r.IdOrder,
                OrderCode = r.Order != null ? r.Order.OrderCode : "",
                r.FoodRating,
                r.DriverRating,
                r.CommentForRes,
                r.CommentForShipper,
                r.CreatedAt,
                CustomerName = r.User?.FullName ?? "Khách hàng ẩn danh",
                CustomerAvatar = r.User?.Avatar ?? "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271401/DAPM_32/users/default-user.jpg",
                ReviewFoods = r.ReviewFoods?.Select(rf => new
                {
                    rf.IdReviewFood,
                    rf.Rating,
                    rf.Comment,
                    rf.Image,
                    FoodName = rf.Food?.Name ?? "Món ăn"
                }).ToList()
            }).ToList();
        }

        public async Task<bool> SimulateDeliveryAndReviewAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderFoods)
                .FirstOrDefaultAsync(o => o.IdOrder == orderId);
            if (order == null)
            {
                return false;
            }

            // 1. Update status to completed
            string oldStatus = order.Status;
            order.Status = "completed";
            order.UpdatedAt = DateTime.Now;
            
            // Set driver if none
            if (order.IdDriver == null)
            {
                var driver = await _context.Drivers.FirstOrDefaultAsync();
                if (driver != null)
                {
                    order.IdDriver = driver.IdDriver;
                }
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            // 2. Add a review if it doesn't exist
            var existingReview = await _context.Reviews.FirstOrDefaultAsync(r => r.IdOrder == orderId);
            if (existingReview == null)
            {
                int nextReviewId = await _context.Reviews.AnyAsync() ? await _context.Reviews.MaxAsync(r => r.IdReview) + 1 : 1;
                var review = new Review
                {
                    IdReview = nextReviewId,
                    IdUser = order.IdUser,
                    IdOrder = order.IdOrder,
                    IdRestaurant = order.IdRestaurant,
                    FoodRating = 5.0f,
                    DriverRating = 4.8f,
                    CommentForRes = "Giao hàng nhanh, đồ ăn nóng hổi và rất ngon! Sẽ tiếp tục ủng hộ nhà hàng lần sau.",
                    CommentForShipper = "Tài xế thân thiện, giao hàng nhanh chóng và lịch sự.",
                    CreatedAt = DateTime.Now
                };
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                // Add ReviewFoods
                int nextReviewFoodId = await _context.ReviewFoods.AnyAsync() ? await _context.ReviewFoods.MaxAsync(rf => rf.IdReviewFood) + 1 : 1;
                foreach (var of in order.OrderFoods)
                {
                    var reviewFood = new ReviewFood
                    {
                        IdReviewFood = nextReviewFoodId++,
                        IdReview = nextReviewId,
                        IdFood = of.IdFood,
                        Rating = 5.0f,
                        Comment = "Ngon tuyệt hảo, chuẩn vị nhà làm!",
                        Image = "",
                        Video = ""
                    };
                    _context.ReviewFoods.Add(reviewFood);
                }
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}

