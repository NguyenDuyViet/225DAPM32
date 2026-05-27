using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly CartService _cartService;

        public OrderService(AppDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<List<OrderResponse>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await GetOrdersQuery()
                .Where(o => o.IdUser == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToOrderResponse).ToList();
        }

        public async Task<List<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await GetOrdersQuery()
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToOrderResponse).ToList();
        }

        public async Task<List<OrderResponse>> GetOrdersByRestaurantAsync(int idRestaurant)
        {
            var orders = await GetOrdersQuery()
                .Where(o => o.IdRestaurant == idRestaurant)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToOrderResponse).ToList();
        }

        public async Task<List<OrderResponse>> GetOrdersByDriverAsync(int idDriver, bool history)
        {
            var statuses = history
                ? new[] { "completed", "cancelled", "canceled" }
                : new[] { "confirmed", "delivering", "restaurant_accepted" };

            var orders = await GetOrdersQuery()
                .Where(o => o.IdDriver == idDriver && statuses.Contains(o.Status))
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToOrderResponse).ToList();
        }

        public async Task<List<OrderResponse>> GetOrdersByDriverUserIdAsync(int userId, bool history)
        {
            var driverId = await _context.Drivers
                .Where(d => d.IdUser == userId)
                .Select(d => (int?)d.IdDriver)
                .FirstOrDefaultAsync();

            return driverId.HasValue
                ? await GetOrdersByDriverAsync(driverId.Value, history)
                : new List<OrderResponse>();
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int userId, int idOrder)
        {
            var order = await GetOrdersQuery()
                .FirstOrDefaultAsync(o => o.IdOrder == idOrder && o.IdUser == userId);

            if (order == null)
                throw new KeyNotFoundException("Khong tim thay don hang.");

            return ToOrderResponse(order);
        }

        public async Task<OrderResponse> CreateOrderFromCartAsync(int userId, CreateOrderRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DeliveryAddress))
                throw new InvalidOperationException("Dia chi giao hang khong duoc de trong.");

            var cart = await _cartService.GetCartEntityByUserIdAsync(userId);
            if (cart == null || !cart.CartFoods.Any())
                throw new InvalidOperationException("Gio hang dang trong.");

            var restaurantIds = cart.CartFoods.Select(cf => cf.Food.IdRestaurant).Distinct().ToList();
            if (restaurantIds.Count != 1)
                throw new InvalidOperationException("Mot don hang chi ho tro mon an tu cung mot nha hang.");

            var foodAmount = cart.CartFoods.Sum(cf => cf.Total);
            var discount = await GetVoucherDiscountAsync(userId, request.IdVoucher, foodAmount);
            var now = DateTime.Now;
            var idOrder = await GetNextOrderIdAsync();

            var order = new Order
            {
                IdOrder = idOrder,
                IdUser = userId,
                IdRestaurant = restaurantIds[0],
                IdVoucher = request.IdVoucher,
                OrderCode = $"ORD{idOrder:000000}",
                DeliveryAddress = request.DeliveryAddress.Trim(),
                DeliveryLat = request.DeliveryLat,
                DeliveryLng = request.DeliveryLng,
                PaymentMethod = request.PaymentMethod,
                FoodAmount = foodAmount,
                ShippingFee = request.ShippingFee,
                Discount = discount,
                FinalTotal = Math.Max(0, foodAmount + request.ShippingFee - discount),
                PaymentStatus = request.PaymentMethod.Equals("cash", StringComparison.OrdinalIgnoreCase) ? "unpaid" : "pending",
                Status = "pending",
                Note = request.Note,
                CreatedAt = now,
                UpdatedAt = now,
                OrderFoods = new List<OrderFood>()
            };

            var nextOrderFoodId = await GetNextOrderFoodIdAsync();
            foreach (var cartFood in cart.CartFoods)
            {
                order.OrderFoods.Add(new OrderFood
                {
                    IdOrderFood = nextOrderFoodId++,
                    IdOrder = idOrder,
                    IdFood = cartFood.IdFood,
                    Quantity = cartFood.Quantity,
                    UnitPrice = cartFood.Price,
                    TotalPrice = cartFood.Total,
                    Note = cartFood.Note
                });
            }

            await _context.Orders.AddAsync(order);

            if (request.IdVoucher.HasValue)
            {
                var voucher = await _context.Vouchers.FirstOrDefaultAsync(v => v.IdVoucher == request.IdVoucher.Value);
                if (voucher != null)
                    voucher.Used = true;
            }

            _context.CartFoods.RemoveRange(cart.CartFoods);
            cart.Total = 0;
            cart.UpdateAt = now;

            await _context.SaveChangesAsync();
            return await GetOrderByIdAsync(userId, idOrder);
        }

        public async Task<OrderResponse> CancelOrderAsync(int userId, int idOrder, CancelOrderRequest request)
        {
            var order = await GetOrdersQuery()
                .FirstOrDefaultAsync(o => o.IdOrder == idOrder && o.IdUser == userId);

            if (order == null)
                throw new KeyNotFoundException("Khong tim thay don hang.");

            if (order.Status != "pending")
                throw new InvalidOperationException("Chi co the huy don hang khi nha hang chua xac nhan.");

            var oldStatus = order.Status;
            order.Status = "cancelled";
            order.CancelReason = request.CancelReason;
            order.UpdatedAt = DateTime.Now;

            await AdjustFoodDailyQuantityAsync(order, oldStatus, order.Status);
            await _context.SaveChangesAsync();
            return ToOrderResponse(order);
        }

        public async Task<OrderResponse> UpdateOrderStatusAsync(int idOrder, UpdateOrderStatusRequest request)
        {
            var order = await GetOrdersQuery().FirstOrDefaultAsync(o => o.IdOrder == idOrder);
            if (order == null)
                throw new KeyNotFoundException("Khong tim thay don hang.");

            if (string.IsNullOrWhiteSpace(request.Status))
                throw new InvalidOperationException("Trang thai don hang khong duoc de trong.");

            var oldStatus = order.Status;
            var assignedDriver = order.Driver;
            order.Status = NormalizeStatus(request.Status);
            order.UpdatedAt = DateTime.Now;

            if (request.IdDriver.HasValue)
            {
                var driver = await _context.Drivers.FindAsync(request.IdDriver.Value);
                if (driver == null)
                    throw new KeyNotFoundException("Khong tim thay shipper.");

                order.IdDriver = request.IdDriver.Value;
                order.Driver = driver;
                driver.IsBusy = order.Status is "delivering" or "confirmed";
            }
            else if ((order.Status == "delivering" || order.Status == "completed") && order.IdDriver == null)
            {
                var driver = await _context.Drivers
                    .Include(d => d.User)
                    .OrderBy(d => d.IsBusy)
                    .ThenBy(d => d.TotalOrders)
                    .FirstOrDefaultAsync();
                if (driver != null)
                {
                    order.IdDriver = driver.IdDriver;
                    order.Driver = driver;
                    driver.IsBusy = order.Status == "delivering";
                }
            }
            else if (order.Status != "delivering" && order.Status != "completed")
            {
                if (assignedDriver != null)
                    assignedDriver.IsBusy = false;

                order.IdDriver = null;
            }

            if (order.Status is "cancelled" or "canceled")
                order.CancelReason = request.CancelReason;

            if (order.Status == "completed" && order.Driver != null)
            {
                order.PaymentStatus = "paid";
                order.Driver.IsBusy = false;
                order.Driver.TotalOrders += 1;
            }

            await AdjustFoodDailyQuantityAsync(order, oldStatus, order.Status);
            await _context.SaveChangesAsync();
            return ToOrderResponse(order);
        }

        public async Task<OrderResponse> UpdateOrderStatusForDriverAsync(int userId, int idOrder, UpdateOrderStatusRequest request)
        {
            var ownsOrder = await _context.Orders
                .AnyAsync(o => o.IdOrder == idOrder && o.Driver != null && o.Driver.IdUser == userId);

            if (!ownsOrder)
                throw new KeyNotFoundException("Khong tim thay don hang duoc giao cho shipper.");

            var status = NormalizeStatus(request.Status);
            if (status is not "delivering" and not "completed")
                throw new InvalidOperationException("Shipper chi co the cap nhat trang thai giao hang.");

            request.Status = status;
            return await UpdateOrderStatusAsync(idOrder, request);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            await UpdateOrderStatusAsync(orderId, new UpdateOrderStatusRequest { Status = status });
            return true;
        }

        public async Task<bool> UpdateOrderAsync(int orderId, OrderRequest dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderFoods)
                .FirstOrDefaultAsync(o => o.IdOrder == orderId);

            if (order == null)
                return false;

            if (dto.DeliveryAddress != null)
                order.DeliveryAddress = dto.DeliveryAddress;

            if (dto.Note != null)
                order.Note = dto.Note;

            if (dto.Status != null)
            {
                var oldStatus = order.Status;
                order.Status = NormalizeStatus(dto.Status);
                await AdjustFoodDailyQuantityAsync(order, oldStatus, order.Status);
            }

            order.UpdatedAt = DateTime.Now;
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
                return false;

            if (order.OrderFoods != null && order.OrderFoods.Any())
                _context.OrderFoods.RemoveRange(order.OrderFoods);

            if (order.OrderPromotions != null && order.OrderPromotions.Any())
                _context.OrderPromotions.RemoveRange(order.OrderPromotions);

            if (order.Review != null)
            {
                if (order.Review.ReviewFoods != null && order.Review.ReviewFoods.Any())
                    _context.ReviewFoods.RemoveRange(order.Review.ReviewFoods);

                _context.Reviews.Remove(order.Review);
            }

            if (order.PaymentMethodEntity != null)
                _context.PaymentMethods.Remove(order.PaymentMethodEntity);

            if (order.Complaint != null)
                _context.Complaints.Remove(order.Complaint);

            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<object> GetDashboardStatsAsync(int restaurantId)
        {
            var today = DateTime.Today;
            var allRestaurantOrders = await _context.Orders
                .Where(o => o.IdRestaurant == restaurantId)
                .ToListAsync();

            var todayOrders = allRestaurantOrders.Where(o => o.CreatedAt.Date == today).ToList();
            var reviews = await _context.Reviews
                .Where(r => r.IdRestaurant == restaurantId)
                .ToListAsync();

            var recentOrders = await _context.Orders
                .Where(o => o.IdRestaurant == restaurantId)
                .Include(o => o.User)
                .Include(o => o.OrderFoods)
                    .ThenInclude(of => of.Food)
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .ToListAsync();

            var popularFoods = await _context.Foods
                .Where(f => f.IdRestaurant == restaurantId)
                .OrderByDescending(f => f.CookCount)
                .Take(4)
                .ToListAsync();

            return new
            {
                TotalOrdersToday = todayOrders.Count,
                RevenueToday = todayOrders.Where(o => o.Status != "canceled" && o.Status != "cancelled").Sum(o => o.FinalTotal),
                PreparingOrders = allRestaurantOrders.Count(o => o.Status != "completed" && o.Status != "canceled" && o.Status != "cancelled"),
                Rating = reviews.Any() ? (decimal)reviews.Average(r => r.FoodRating) : 5m,
                RecentOrders = recentOrders.Select(o => new
                {
                    o.IdOrder,
                    o.OrderCode,
                    CustomerName = o.User?.FullName ?? "Khach hang",
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

            var dailyRevenueList = new List<decimal>();
            var dayLabels = new List<string>();
            string[] viDays = { "Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };

            foreach (var day in past7Days)
            {
                dailyRevenueList.Add(orders
                    .Where(o => o.CreatedAt.Date == day.Date && o.Status != "canceled" && o.Status != "cancelled")
                    .Sum(o => o.FinalTotal));
                dayLabels.Add(viDays[(int)day.DayOfWeek]);
            }

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

        public async Task<Order> SeedOrderAsync()
        {
            var restaurant = await _context.Restaurants.FindAsync(1);
            if (restaurant == null)
            {
                restaurant = new Restaurant
                {
                    IdRestaurant = 1,
                    NameRestaurant = "Bep Nha Viet",
                    Description = "Nha hang am thuc truyen thong Viet Nam",
                    Address = "123 Duong Ba Thang Hai, Quan 10, TP.HCM",
                    OpenTime = TimeSpan.Parse("08:00:00"),
                    CloseTime = TimeSpan.Parse("22:00:00")
                };
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
            }

            var category = await _context.Categories.FirstOrDefaultAsync();
            if (category == null)
            {
                category = new Category
                {
                    IdCategory = 1,
                    Name = "Mon Chinh",
                    Icon = "fast-food"
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            var foods = await _context.Foods.Where(f => f.IdRestaurant == 1).ToListAsync();
            if (!foods.Any())
            {
                var maxFoodId = await _context.Foods.AnyAsync() ? await _context.Foods.MaxAsync(f => f.IdFood) : 0;
                var food1 = new Food
                {
                    IdFood = Math.Max(1, maxFoodId + 1),
                    IdRestaurant = 1,
                    IdCategory = category.IdCategory,
                    Name = "Banh xeo Bep Nha Viet",
                    Price = 65000,
                    Discount = 0,
                    Description = "Banh xeo gion rum voi nhan tom thit",
                    Image = "https://images.unsplash.com/photo-1583032353423-048ba6b11c20?w=500",
                    CookCount = 10,
                    PrepTime = 15
                };

                var food2 = new Food
                {
                    IdFood = food1.IdFood + 1,
                    IdRestaurant = 1,
                    IdCategory = category.IdCategory,
                    Name = "Ca kho to dat set",
                    Price = 85000,
                    Discount = 0,
                    Description = "Ca loc kho to dam da huong vi mien Tay",
                    Image = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=500",
                    CookCount = 8,
                    PrepTime = 20
                };

                _context.Foods.AddRange(food1, food2);
                await _context.SaveChangesAsync();
                foods.AddRange(new[] { food1, food2 });
            }

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
                    FullName = "Nguyen Van An",
                    Phone = "0987654321",
                    Email = "customer@example.com",
                    Address = "456 Le Duan, Quan 1, TP.HCM",
                    Status = "active",
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var nextOrderId = await GetNextOrderIdAsync();
            var order = new Order
            {
                IdOrder = nextOrderId,
                IdUser = user.IdUser,
                IdRestaurant = 1,
                OrderCode = "DH-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                DeliveryAddress = "789 Dien Bien Phu, Phuong 25, Quan Binh Thanh, TP.HCM",
                PaymentMethod = "COD",
                PaymentStatus = "pending",
                Status = "pending",
                Note = "Giao hang gio hanh chinh, goi truoc khi den giup minh nhe!",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FoodAmount = 0,
                ShippingFee = 15000,
                Discount = 0,
                FinalTotal = 0,
                OrderFoods = new List<OrderFood>()
            };

            var nextOrderFoodId = await GetNextOrderFoodIdAsync();
            decimal totalFoodAmount = 0;
            var foodIndex = 0;

            foreach (var food in foods.Take(2))
            {
                var qty = foodIndex == 0 ? 2 : 1;
                var totalPrice = food.Price * qty;
                totalFoodAmount += totalPrice;

                order.OrderFoods.Add(new OrderFood
                {
                    IdOrderFood = nextOrderFoodId++,
                    IdOrder = nextOrderId,
                    IdFood = food.IdFood,
                    Quantity = qty,
                    UnitPrice = food.Price,
                    TotalPrice = totalPrice,
                    Note = foodIndex == 0 ? "Nhieu rau, khong cay" : "It hanh"
                });
                foodIndex++;
            }

            order.FoodAmount = totalFoodAmount;
            order.FinalTotal = totalFoodAmount + order.ShippingFee - order.Discount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
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
                CustomerName = r.User?.FullName ?? "Khach hang an danh",
                CustomerAvatar = r.User?.Avatar ?? "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271401/DAPM_32/users/default-user.jpg",
                ReviewFoods = r.ReviewFoods?.Select(rf => new
                {
                    rf.IdReviewFood,
                    rf.Rating,
                    rf.Comment,
                    rf.Image,
                    FoodName = rf.Food?.Name ?? "Mon an"
                }).ToList()
            }).ToList();
        }

        public async Task<bool> SimulateDeliveryAndReviewAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderFoods)
                .FirstOrDefaultAsync(o => o.IdOrder == orderId);

            if (order == null)
                return false;

            var oldStatus = order.Status;
            order.Status = "completed";
            order.UpdatedAt = DateTime.Now;

            if (order.IdDriver == null)
            {
                var driver = await _context.Drivers.FirstOrDefaultAsync();
                if (driver != null)
                    order.IdDriver = driver.IdDriver;
            }

            await AdjustFoodDailyQuantityAsync(order, oldStatus, order.Status);
            await _context.SaveChangesAsync();

            var existingReview = await _context.Reviews.FirstOrDefaultAsync(r => r.IdOrder == orderId);
            if (existingReview != null)
                return true;

            var nextReviewId = await _context.Reviews.AnyAsync() ? await _context.Reviews.MaxAsync(r => r.IdReview) + 1 : 1;
            var review = new Review
            {
                IdReview = nextReviewId,
                IdUser = order.IdUser,
                IdOrder = order.IdOrder,
                IdRestaurant = order.IdRestaurant,
                FoodRating = 5.0f,
                DriverRating = 4.8f,
                CommentForRes = "Giao hang nhanh, do an nong hoi va rat ngon.",
                CommentForShipper = "Tai xe than thien, giao hang nhanh chong va lich su.",
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var nextReviewFoodId = await _context.ReviewFoods.AnyAsync() ? await _context.ReviewFoods.MaxAsync(rf => rf.IdReviewFood) + 1 : 1;
            foreach (var orderFood in order.OrderFoods)
            {
                _context.ReviewFoods.Add(new ReviewFood
                {
                    IdReviewFood = nextReviewFoodId++,
                    IdReview = nextReviewId,
                    IdFood = orderFood.IdFood,
                    Rating = 5.0f,
                    Comment = "Ngon tuyet hao, chuan vi nha lam!",
                    Image = "",
                    Video = ""
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<decimal> GetVoucherDiscountAsync(int userId, int? idVoucher, decimal foodAmount)
        {
            if (!idVoucher.HasValue)
                return 0;

            var voucher = await _context.Vouchers.FirstOrDefaultAsync(v =>
                v.IdVoucher == idVoucher.Value &&
                v.IdUser == userId &&
                !v.Used &&
                v.Expiry >= DateTime.Now);

            if (voucher == null)
                throw new InvalidOperationException("Voucher khong hop le hoac da het han.");

            return Math.Min(voucher.Value, foodAmount);
        }

        private async Task AdjustFoodDailyQuantityAsync(Order order, string oldStatus, string newStatus)
        {
            if (newStatus == "delivering" && oldStatus != "delivering")
            {
                foreach (var orderFood in order.OrderFoods ?? new List<OrderFood>())
                {
                    var food = await _context.Foods.FindAsync(orderFood.IdFood);
                    if (food != null)
                        food.DailyQuantity = Math.Max(0, food.DailyQuantity - orderFood.Quantity);
                }
            }
            else if ((newStatus == "canceled" || newStatus == "cancelled") && (oldStatus == "delivering" || oldStatus == "completed"))
            {
                foreach (var orderFood in order.OrderFoods ?? new List<OrderFood>())
                {
                    var food = await _context.Foods.FindAsync(orderFood.IdFood);
                    if (food != null)
                        food.DailyQuantity += orderFood.Quantity;
                }
            }
        }

        private static OrderResponse ToOrderResponse(Order order)
        {
            return new OrderResponse
            {
                IdOrder = order.IdOrder,
                OrderCode = order.OrderCode,
                IdUser = order.IdUser,
                IdRestaurant = order.IdRestaurant,
                RestaurantName = order.Restaurant?.NameRestaurant ?? string.Empty,
                DeliveryAddress = order.DeliveryAddress,
                DeliveryLat = order.DeliveryLat,
                DeliveryLng = order.DeliveryLng,
                CustomerName = order.User?.FullName,
                CustomerPhone = order.User?.Phone,
                PaymentMethod = order.PaymentMethod,
                FoodAmount = order.FoodAmount,
                Total = order.FoodAmount,
                ShippingFee = order.ShippingFee,
                Discount = order.Discount,
                FinalTotal = order.FinalTotal,
                PaymentStatus = order.PaymentStatus,
                Status = order.Status,
                Note = order.Note,
                CancelReason = order.CancelReason,
                DriverName = order.Driver?.User?.FullName,
                DriverPhone = order.Driver?.User?.Phone,
                EstimatedDelivery = EstimateDelivery(order),
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Items = (order.OrderFoods ?? new List<OrderFood>()).Select(of => new OrderItemResponse
                {
                    IdOrderFood = of.IdOrderFood,
                    IdFood = of.IdFood,
                    FoodName = of.Food?.Name ?? string.Empty,
                    FoodImage = of.Food?.Image ?? string.Empty,
                    Quantity = of.Quantity,
                    UnitPrice = of.UnitPrice,
                    TotalPrice = of.TotalPrice,
                    Note = of.Note
                }).ToList()
            };
        }

        private IQueryable<Order> GetOrdersQuery()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.Driver)
                    .ThenInclude(d => d.User)
                .Include(o => o.OrderFoods)
                    .ThenInclude(of => of.Food);
        }

        private static DateTime? EstimateDelivery(Order order)
        {
            return order.Status is "completed" or "cancelled" or "canceled"
                ? order.UpdatedAt
                : order.CreatedAt.AddMinutes(45);
        }

        private static string NormalizeStatus(string status)
        {
            return status.Trim().ToLowerInvariant() switch
            {
                "canceled" => "cancelled",
                "accepted" => "confirmed",
                "restaurant_accepted" => "confirmed",
                var value => value
            };
        }

        private async Task<int> GetNextOrderIdAsync()
        {
            return await _context.Orders.AnyAsync()
                ? await _context.Orders.MaxAsync(o => o.IdOrder) + 1
                : 1;
        }

        private async Task<int> GetNextOrderFoodIdAsync()
        {
            return await _context.OrderFoods.AnyAsync()
                ? await _context.OrderFoods.MaxAsync(of => of.IdOrderFood) + 1
                : 1;
        }
    }
}
