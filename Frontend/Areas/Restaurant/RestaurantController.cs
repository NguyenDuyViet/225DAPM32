using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using _225DAPM32.Areas.Restaurant.Models;
using _225DAPM32.Models;
using RestaurantEntity = _225DAPM32.Models.Restaurant;

namespace _225DAPM32.Areas.Restaurant
{
    [Area("Restaurant")]
    public class RestaurantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public RestaurantController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private HttpClient GetApiClient()
        {
            var client = _httpClientFactory.CreateClient("API");
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        private int GetRestaurantId()
        {
            // Default to restaurant ID 1 ("Bếp Nhà Việt") for demo/portal access
            return HttpContext.Session.GetInt32("RestaurantId") ?? 1;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard Nhà hàng";
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            var model = new RestaurantDashboardViewModel
            {
                ActiveOrders = 0,
                RevenueToday = 0,
                MenuItems = 0,
                Rating = 5.0m
            };

            try
            {
                // Fetch stats from Backend
                var response = await client.GetAsync($"Restaurants/{restaurantId}/dashboard");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<BackendDashboardStats>>(content, _jsonOptions);

                    if (apiResponse?.Results != null)
                    {
                        var stats = apiResponse.Results;
                        model.ActiveOrders = stats.PreparingOrders;
                        model.RevenueToday = stats.RevenueToday;
                        model.Rating = stats.Rating;

                        ViewBag.TotalOrdersToday = stats.TotalOrdersToday;
                        ViewBag.RecentOrders = stats.RecentOrders;
                        ViewBag.PopularItems = stats.PopularItems;
                    }
                }

                // Fetch menu item count (foods count of this restaurant)
                var foodResponse = await client.GetAsync($"Food/restaurant/{restaurantId}");
                if (foodResponse.IsSuccessStatusCode)
                {
                    var foodContent = await foodResponse.Content.ReadAsStringAsync();
                    var foodApiResponse = JsonSerializer.Deserialize<ApiResponse<List<Food>>>(foodContent, _jsonOptions);
                    model.MenuItems = foodApiResponse?.Results?.Count ?? 0;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi kết nối Backend: {ex.Message}";
            }

            return View(model);
        }

        public async Task<IActionResult> Chat()
        {
            ViewData["Title"] = "Chat";
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            // Load restaurant orders to build room list (each order = a customer room)
            var orderRooms = new List<ChatRoomInfo>();
            try
            {
                var response = await client.GetAsync($"Orders/restaurant/{restaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<BackendOrder>>>(content, _jsonOptions);
                    if (apiResponse?.Results != null)
                    {
                        foreach (var order in apiResponse.Results.Take(30))
                        {
                            var roomId = $"order_{order.IdOrder}";
                            var partnerRole = order.Status switch
                            {
                                "preparing" or "ready" or "delivering" => "shipper",
                                _ => "customer"
                            };
                            var partnerName = partnerRole == "shipper"
                                ? (order.Driver?.User?.FullName ?? $"Shipper #{order.IdOrder}")
                                : (order.User?.FullName ?? $"Khách #{order.IdOrder}");

                            orderRooms.Add(new ChatRoomInfo
                            {
                                RoomId = roomId,
                                OrderId = order.IdOrder,
                                OrderCode = order.OrderCode ?? $"#{order.IdOrder}",
                                PartnerName = partnerName,
                                PartnerRole = partnerRole,
                                Status = order.Status,
                                LastMessage = "",
                                LastTime = order.CreatedAt
                            });
                        }
                    }
                }
            }
            catch { }

            ViewBag.RestaurantId = restaurantId;
            ViewBag.ChatRooms = orderRooms;
            return View();
        }

        public async Task<IActionResult> Orders()
        {
            ViewData["Title"] = "Đơn hàng";
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();
            var orders = new List<Order>();

            try
            {
                var response = await client.GetAsync($"Orders/restaurant/{restaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<BackendOrder>>>(content, _jsonOptions);

                    if (apiResponse?.Results != null)
                    {
                        orders = apiResponse.Results.Select(bo => new Order
                        {
                            IdOrder = bo.IdOrder,
                            IdUser = bo.IdUser,
                            Total = bo.FoodAmount,
                            ShippingFee = bo.ShippingFee,
                            Discount = bo.Discount,
                            FinalTotal = bo.FinalTotal,
                            Status = bo.Status,
                            Note = bo.Note ?? "",
                            CreatedAt = bo.CreatedAt,
                            DriverName = bo.Driver?.User?.FullName ?? "Chưa có",
                            DriverPhone = bo.Driver?.User?.Phone ?? "",
                            User = new User { FullName = bo.User?.FullName ?? "Khách hàng" },
                            Address = new Address { AddressDetail = bo.DeliveryAddress ?? "" },
                            OrderFoods = bo.OrderFoods?.Select(of => new OrderFoodViewModel
                            {
                                IdFood = of.IdFood,
                                Name = of.Food?.Name ?? "Món ăn",
                                Quantity = of.Quantity,
                                UnitPrice = of.UnitPrice,
                                TotalPrice = of.TotalPrice,
                                Note = of.Note,
                                Image = of.Food?.Image ?? "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271401/DAPM_32/foods/food-default.jpg"
                            }).ToList()
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi tải đơn hàng: {ex.Message}";
            }

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var client = GetApiClient();
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(status), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Orders/{orderId}/status", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật trạng thái đơn hàng thành công!";
                }
                else
                {
                    TempData["Error"] = "Cập nhật trạng thái thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Orders", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder(int orderId, string address, string note, string status)
        {
            var client = GetApiClient();
            try
            {
                var updateDto = new
                {
                    DeliveryAddress = (string)null,
                    Note = (string)null,
                    Status = status
                };
                var jsonContent = new StringContent(JsonSerializer.Serialize(updateDto), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Orders/{orderId}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật đơn hàng thành công!";
                }
                else
                {
                    TempData["Error"] = "Cập nhật đơn hàng thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Orders", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var client = GetApiClient();
            try
            {
                var response = await client.DeleteAsync($"Orders/{orderId}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xóa đơn hàng thành công!";
                }
                else
                {
                    TempData["Error"] = "Xóa đơn hàng thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Orders", new { area = "Restaurant" });
        }

        public async Task<IActionResult> Analytics()
        {
            ViewData["Title"] = "Thống kê";
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                var response = await client.GetAsync($"Restaurants/{restaurantId}/analytics");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<BackendAnalyticsStats>>(content, _jsonOptions);

                    if (apiResponse?.Results != null)
                    {
                        ViewBag.DayLabels = apiResponse.Results.DayLabels;
                        ViewBag.RevenueData = apiResponse.Results.RevenueData;
                        ViewBag.StatusLabels = apiResponse.Results.StatusLabels;
                        ViewBag.StatusData = apiResponse.Results.StatusData;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi tải thống kê: {ex.Message}";
            }

            return View();
        }

        public async Task<IActionResult> Profile()
        {
            ViewData["Title"] = "Quản lý Nhà hàng";
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();
            var model = new RestaurantEntity();

            try
            {
                // 1. Fetch Restaurant Entity
                var response = await client.GetAsync($"Restaurants/{restaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<RestaurantEntity>>(content, _jsonOptions);
                    model = apiResponse?.Results ?? new RestaurantEntity();
                }

                // 2. Fetch Foods
                var foodResponse = await client.GetAsync($"Food/restaurant/{restaurantId}");
                var foods = new List<Food>();
                if (foodResponse.IsSuccessStatusCode)
                {
                    var foodContent = await foodResponse.Content.ReadAsStringAsync();
                    var foodApiResponse = JsonSerializer.Deserialize<ApiResponse<List<Food>>>(foodContent, _jsonOptions);
                    foods = foodApiResponse?.Results ?? new List<Food>();
                }
                ViewBag.Foods = foods;

                // 3. Fetch Categories
                var categoryResponse = await client.GetAsync("Category");
                var categories = new List<Category>();
                if (categoryResponse.IsSuccessStatusCode)
                {
                    var categoryContent = await categoryResponse.Content.ReadAsStringAsync();
                    var categoryApiResponse = JsonSerializer.Deserialize<ApiResponse<List<Category>>>(categoryContent, _jsonOptions);
                    categories = categoryApiResponse?.Results ?? new List<Category>();
                }
                ViewBag.Categories = categories;

                // 4. Fetch Promotions
                var promoResponse = await client.GetAsync($"Promotion/restaurant/{restaurantId}");
                var promotions = new List<Promotion>();
                if (promoResponse.IsSuccessStatusCode)
                {
                    var promoContent = await promoResponse.Content.ReadAsStringAsync();
                    var promoApiResponse = JsonSerializer.Deserialize<ApiResponse<List<Promotion>>>(promoContent, _jsonOptions);
                    promotions = promoApiResponse?.Results ?? new List<Promotion>();
                }
                ViewBag.Promotions = promotions;
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi tải dữ liệu nhà hàng: {ex.Message}";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(int idCategory, string name, string description, string image, string video, decimal price, decimal? discount, int? prepTime)
        {
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                var request = new
                {
                    IdCategory = idCategory,
                    IdRestaurant = restaurantId,
                    Name = name,
                    Description = description ?? "",
                    Image = image ?? "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271401/DAPM_32/foods/food-default.jpg",
                    Video = video ?? "",
                    Price = price,
                    Discount = discount ?? 0,
                    PrepTime = prepTime ?? 15
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Food", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm món ăn mới thành công!";
                }
                else
                {
                    TempData["Error"] = "Thêm món ăn thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Profile", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFood(int idFood, int idCategory, string name, string description, string image, string video, decimal price, decimal? discount, int? prepTime)
        {
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                var request = new
                {
                    IdCategory = idCategory,
                    IdRestaurant = restaurantId,
                    Name = name,
                    Description = description ?? "",
                    Image = image ?? "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271401/DAPM_32/foods/food-default.jpg",
                    Video = video ?? "",
                    Price = price,
                    Discount = discount ?? 0,
                    PrepTime = prepTime ?? 15
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Food/{idFood}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật món ăn thành công!";
                }
                else
                {
                    TempData["Error"] = "Cập nhật món ăn thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Profile", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFood(int idFood)
        {
            var client = GetApiClient();

            try
            {
                var response = await client.DeleteAsync($"Food/{idFood}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xóa món ăn thành công!";
                }
                else
                {
                    TempData["Error"] = "Xóa món ăn thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Profile", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromotion(string type, decimal value, decimal minOrderValue, decimal maxDiscount, int usageLimit, string startDate, string endDate)
        {
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                DateTime.TryParse(startDate, out var parsedStart);
                DateTime.TryParse(endDate, out var parsedEnd);

                var request = new
                {
                    Type = type,
                    Value = value,
                    MinOrderValue = minOrderValue,
                    MaxDiscount = maxDiscount,
                    UsageLimit = usageLimit,
                    UsedCount = 0,
                    StartDate = parsedStart,
                    EndDate = parsedEnd,
                    IdRestaurant = restaurantId
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Promotion", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm chương trình khuyến mãi thành công!";
                }
                else
                {
                    TempData["Error"] = "Thêm chương trình khuyến mãi thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Profile", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePromotion(int idPromo, string type, decimal value, decimal minOrderValue, decimal maxDiscount, int usageLimit, string startDate, string endDate, int usedCount)
        {
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                DateTime.TryParse(startDate, out var parsedStart);
                DateTime.TryParse(endDate, out var parsedEnd);

                var request = new
                {
                    Type = type,
                    Value = value,
                    MinOrderValue = minOrderValue,
                    MaxDiscount = maxDiscount,
                    UsageLimit = usageLimit,
                    UsedCount = usedCount,
                    StartDate = parsedStart,
                    EndDate = parsedEnd,
                    IdRestaurant = restaurantId
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Promotion/{idPromo}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật chương trình khuyến mãi thành công!";
                }
                else
                {
                    TempData["Error"] = "Cập nhật chương trình khuyến mãi thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Profile", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePromotion(int idPromo)
        {
            var client = GetApiClient();

            try
            {
                var response = await client.DeleteAsync($"Promotion/{idPromo}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xóa chương trình khuyến mãi thành công!";
                }
                else
                {
                    TempData["Error"] = "Xóa chương trình khuyến mãi thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Profile", new { area = "Restaurant" });
        }

        public async Task<IActionResult> Settings()
        {
            ViewData["Title"] = "Cài đặt";
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();
            var model = new RestaurantEntity();

            try
            {
                var response = await client.GetAsync($"Restaurants/{restaurantId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<RestaurantEntity>>(content, _jsonOptions);
                    model = apiResponse?.Results ?? new RestaurantEntity();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi tải cài đặt: {ex.Message}";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettings(string nameRestaurant, string description, string address, string image, string openTime, string closeTime)
        {
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                TimeSpan.TryParse(openTime, out var parsedOpen);
                TimeSpan.TryParse(closeTime, out var parsedClose);

                var updateDto = new
                {
                    NameRestaurant = nameRestaurant,
                    Description = description,
                    Address = address,
                    Image = image ?? "https://res.cloudinary.com/dzrhf1hwk/image/upload/v1779271401/DAPM_32/restaurants/restaurant-1.jpg",
                    OpenTime = parsedOpen,
                    CloseTime = parsedClose,
                    Lat = 10.782500m,
                    Lng = 106.700000m
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(updateDto), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Restaurants/{restaurantId}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Lưu cài đặt nhà hàng thành công!";
                }
                else
                {
                    TempData["Error"] = "Lưu cài đặt thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Settings", new { area = "Restaurant" });
        }

        // --- Helper Models to deserialize Backend Responses ---
        public class ApiResponse<T>
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public T Results { get; set; }
        }

        public class BackendDashboardStats
        {
            public int TotalOrdersToday { get; set; }
            public decimal RevenueToday { get; set; }
            public int PreparingOrders { get; set; }
            public decimal Rating { get; set; }
            public List<BackendRecentOrder> RecentOrders { get; set; }
            public List<BackendPopularItem> PopularItems { get; set; }
        }

        public class BackendRecentOrder
        {
            public int IdOrder { get; set; }
            public string OrderCode { get; set; }
            public string CustomerName { get; set; }
            public decimal FinalTotal { get; set; }
            public string Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public string ItemsText { get; set; }
        }

        public class BackendPopularItem
        {
            public int IdFood { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Image { get; set; }
            public int CookCount { get; set; }
        }

        public class BackendAnalyticsStats
        {
            public List<string> DayLabels { get; set; }
            public List<decimal> RevenueData { get; set; }
            public List<string> StatusLabels { get; set; }
            public List<int> StatusData { get; set; }
        }

        public class BackendOrder
        {
            public int IdOrder { get; set; }
            public int IdUser { get; set; }
            public string? OrderCode { get; set; }
            public string? DeliveryAddress { get; set; }
            public decimal FoodAmount { get; set; }
            public decimal ShippingFee { get; set; }
            public decimal Discount { get; set; }
            public decimal FinalTotal { get; set; }
            public string Status { get; set; }
            public string? Note { get; set; }
            public DateTime CreatedAt { get; set; }
            public BackendUser? User { get; set; }
            public BackendDriver? Driver { get; set; }
            public List<BackendOrderFood>? OrderFoods { get; set; }
        }

        public class BackendOrderFood
        {
            public int IdOrderFood { get; set; }
            public int IdFood { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
            public string? Note { get; set; }
            public BackendFood? Food { get; set; }
        }

        public class BackendFood
        {
            public int IdFood { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Image { get; set; }
        }

        public class BackendUser
        {
            public int IdUser { get; set; }
            public string FullName { get; set; }
            public string Phone { get; set; }
        }

        public class BackendDriver
        {
            public int IdDriver { get; set; }
            public BackendUser? User { get; set; }
        }

        public class ChatRoomInfo
        {
            public string RoomId { get; set; } = string.Empty;
            public int OrderId { get; set; }
            public string OrderCode { get; set; } = string.Empty;
            public string PartnerName { get; set; } = string.Empty;
            public string PartnerRole { get; set; } = "customer"; // "customer" | "shipper"
            public string Status { get; set; } = string.Empty;
            public string LastMessage { get; set; } = string.Empty;
            public DateTime LastTime { get; set; }
        }
    }
}