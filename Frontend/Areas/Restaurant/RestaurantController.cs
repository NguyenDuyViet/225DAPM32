using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using _225DAPM32.Areas.Restaurant.Models;
using _225DAPM32.Models;
using _225DAPM32.Services;
using RestaurantEntity = _225DAPM32.Models.Restaurant;

namespace _225DAPM32.Areas.Restaurant
{
    [Area("Restaurant")]
    public class RestaurantController : Controller
    {
        private readonly ApiClient _apiClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public RestaurantController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            if (!RequireRestaurant())
                return RedirectToAction("Index", "Home", new { area = "" });

            var restaurant = await GetManagedRestaurantAsync();
            var foods = restaurant == null
                ? new List<Food>()
                : await _apiClient.GetResultAsync<List<Food>>($"Food/restaurant/{restaurant.IdRestaurant}", false) ?? new List<Food>();
            
            var orders = restaurant == null
                ? new List<Order>()
                : await _apiClient.GetResultAsync<List<Order>>($"Orders/restaurant/{restaurant.IdRestaurant}") ?? new List<Order>();

            ViewData["Title"] = "Dashboard Nhà hàng";
            return View(new RestaurantDashboardViewModel
            {
                ActiveOrders = orders.Count(o => o.Status != "completed" && o.Status != "canceled"),
                RevenueToday = orders.Where(o => o.CreatedAt.Date == DateTime.Today).Sum(o => o.FinalTotal),
                MenuItems = foods.Count,
                Rating = 4.7m
            });
        }

        public async Task<IActionResult> Orders()
        {
            if (!RequireRestaurant())
                return RedirectToAction("Index", "Home", new { area = "" });

            var restaurant = await GetManagedRestaurantAsync();
            ViewData["Title"] = "Đơn hàng";

            if (restaurant == null)
                return View(new List<Order>());

            return View(await _apiClient.GetResultAsync<List<Order>>($"Orders/restaurant/{restaurant.IdRestaurant}") ?? new List<Order>());
        }
        
        public IActionResult Chat()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatusWithVerify(int orderId, string status, string verifyCode)
        {
            var client = GetApiClient();
            try
            {
                var getResponse = await client.GetAsync($"Orders/restaurant/{GetRestaurantId()}");
                if (getResponse.IsSuccessStatusCode)
                {
                    var content = await getResponse.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<BackendOrder>>>(content, _jsonOptions);
                    var order = apiResponse?.Results?.FirstOrDefault(o => o.IdOrder == orderId);

                    if (order != null)
                    {
                        var expectedCode = order.IdOrder.ToString();
                        var expectedCode2 = order.OrderCode ?? "";
                        
                        var isCodeValid = verifyCode == expectedCode || 
                                          (!string.IsNullOrEmpty(expectedCode2) && expectedCode2.EndsWith(verifyCode, StringComparison.OrdinalIgnoreCase)) ||
                                          (!string.IsNullOrEmpty(expectedCode2) && expectedCode2.Equals(verifyCode, StringComparison.OrdinalIgnoreCase));

                        if (!isCodeValid)
                        {
                            TempData["Error"] = "Mã xác nhận đơn hàng không chính xác! Vui lòng kiểm tra lại.";
                            return RedirectToAction("Orders", new { area = "Restaurant" });
                        }
                    }
                }

                var jsonContent = new StringContent(JsonSerializer.Serialize(status), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Orders/{orderId}/status", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xác nhận mã đơn hàng thành công! Đã bàn giao cho tài xế.";
                }
                else
                {
                    TempData["Error"] = "Bàn giao thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Orders", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> SimulateDelivery(int orderId)
        {
            var client = GetApiClient();
            try
            {
                var response = await client.PostAsync($"Orders/{orderId}/simulate-delivery-and-review", null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Mô phỏng Shipper giao hàng thành công & khách hàng đã gửi đánh giá!";
                }
                else
                {
                    TempData["Error"] = "Mô phỏng thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Orders", new { area = "Restaurant" });
        }

        [HttpPost]
        public async Task<IActionResult> SimulateOrder()
        {
            var client = GetApiClient();
            try
            {
                var response = await client.PostAsync("Orders/seed", null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Tạo đơn hàng giả lập thành công!";
                }
                else
                {
                    TempData["Error"] = "Tạo đơn hàng giả lập thất bại!";
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
            if (!RequireRestaurant())
                return RedirectToAction("Index", "Home", new { area = "" });

            ViewData["Title"] = "Thông tin Nhà hàng";
            return View(await GetManagedRestaurantAsync() ?? new RestaurantEntity());
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(int idCategory, string name, string description, string image, string video, decimal price, decimal? discount, int? prepTime, int dailyQuantity)
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
                    PrepTime = prepTime ?? 15,
                    DailyQuantity = dailyQuantity
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
        public async Task<IActionResult> UpdateFood(int idFood, int idCategory, string name, string description, string image, string video, decimal price, decimal? discount, int? prepTime, int dailyQuantity)
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
                    PrepTime = prepTime ?? 15,
                    DailyQuantity = dailyQuantity
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
        public async Task<IActionResult> UpdateDailyQuantity(int idFood, int dailyQuantity)
        {
            var client = GetApiClient();
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(dailyQuantity), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Food/{idFood}/daily-quantity", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật số lượng bán trong ngày thành công!";
                }
                else
                {
                    TempData["Error"] = "Cập nhật số lượng thất bại!";
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

        // --- Helper Methods ---
        private HttpClient GetApiClient()
        {
            var client = new HttpClient();
            // Cập nhật lại port nếu Backend của bạn chạy port khác
            client.BaseAddress = new Uri("https://localhost:8000/api/"); 
            return client;
        }

        private int GetRestaurantId()
        {
            // Tạm thời fix cứng là 1 cho mục đích demo, 
            // sau này bạn đổi thành lấy từ Session hoặc User nhé.
            return 1; 
        }

        private async Task<RestaurantEntity?> GetManagedRestaurantAsync()
        {
            var restaurants = await _apiClient.GetResultAsync<List<RestaurantEntity>>("Restaurants/all", false) ?? new List<RestaurantEntity>();
            return restaurants.FirstOrDefault();
        }

        private bool RequireRestaurant()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "restaurant" || role == "admin";
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
            public string PartnerRole { get; set; } = "customer"; 
            public string Status { get; set; } = string.Empty;
            public string LastMessage { get; set; } = string.Empty;
            public DateTime LastTime { get; set; }
        }

        public class ReviewViewModel
        {
            public int IdReview { get; set; }
            public int IdOrder { get; set; }
            public string? OrderCode { get; set; }
            public float FoodRating { get; set; }
            public float DriverRating { get; set; }
            public string? CommentForRes { get; set; }
            public string? CommentForShipper { get; set; }
            public DateTime CreatedAt { get; set; }
            public string? CustomerName { get; set; }
            public string? CustomerAvatar { get; set; }
            public List<ReviewFoodViewModel>? ReviewFoods { get; set; }
        }

        public class ReviewFoodViewModel
        {
            public int IdReviewFood { get; set; }
            public float Rating { get; set; }
            public string? Comment { get; set; }
            public string? Image { get; set; }
            public string? FoodName { get; set; }
        }
    }
}