using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HttpContext.Session.GetString("Role") != "restaurant")
            {
                context.Result = NotFound();
                return;
            }

            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> Index()
        {
            if (!RequireRestaurant())
                return NotFound();

            var restaurant = await GetManagedRestaurantAsync();
            var foods = restaurant == null
                ? new List<Food>()
                : await _apiClient.GetResultAsync<List<Food>>($"Food/restaurant/{restaurant.IdRestaurant}", false) ?? new List<Food>();
            
            var stats = restaurant == null
                ? null
                : await _apiClient.GetResultAsync<BackendDashboardStats>($"Restaurants/{restaurant.IdRestaurant}/dashboard");

            if (stats == null)
            {
                stats = new BackendDashboardStats
                {
                    TotalOrdersToday = 0,
                    RevenueToday = 0,
                    PreparingOrders = 0,
                    Rating = 5.0m,
                    RecentOrders = new List<BackendRecentOrder>(),
                    PopularItems = new List<BackendPopularItem>()
                };
            }

            ViewBag.TotalOrdersToday = stats.TotalOrdersToday;
            ViewBag.RecentOrders = stats.RecentOrders ?? new List<BackendRecentOrder>();
            ViewBag.PopularItems = stats.PopularItems ?? new List<BackendPopularItem>();

            ViewData["Title"] = "Dashboard Nhà hàng";
            return View(new RestaurantDashboardViewModel
            {
                ActiveOrders = stats.PreparingOrders,
                RevenueToday = stats.RevenueToday,
                MenuItems = foods.Count,
                Rating = stats.Rating
            });
        }

        public async Task<IActionResult> Orders(int page = 1)
        {
            if (!RequireRestaurant())
                return NotFound();

            const int pageSize = 5;
            var restaurant = await GetManagedRestaurantAsync();
            ViewData["Title"] = "Đơn hàng";

            if (restaurant == null)
                return View(new List<Order>());

            var orders = await _apiClient.GetResultAsync<List<Order>>($"Orders/restaurant/{restaurant.IdRestaurant}") ?? new List<Order>();

            // Sort orders: Active orders (Status != "completed") sorted by CreatedAt Descending first,
            // then Completed orders (Status == "completed") sorted by CreatedAt Descending.
            var sortedOrders = orders
                .OrderBy(o => o.Status == "completed" ? 1 : 0)
                .ThenByDescending(o => o.CreatedAt)
                .ToList();

            var totalPages = Math.Max(1, (int)Math.Ceiling(sortedOrders.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            ViewBag.RestaurantId = restaurant.IdRestaurant;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalOrders = sortedOrders.Count;

            return View(sortedOrders.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }
        
        public async Task<IActionResult> Chat()
        {
            if (!RequireRestaurant())
                return NotFound();

            var restaurant = await GetManagedRestaurantAsync();
            if (restaurant == null)
            {
                ViewBag.ChatRooms = new List<ChatRoomInfo>();
                return View();
            }

            var orders = await _apiClient.GetResultAsync<List<Order>>($"Orders/restaurant/{restaurant.IdRestaurant}") ?? new List<Order>();
            ViewBag.RestaurantId = restaurant.IdRestaurant;
            ViewBag.ChatRooms = orders
                .OrderByDescending(order => order.UpdatedAt ?? order.CreatedAt)
                .Select(order => new ChatRoomInfo
                {
                    RoomId = $"order_{order.IdOrder}",
                    OrderId = order.IdOrder,
                    OrderCode = string.IsNullOrWhiteSpace(order.OrderCode) ? $"#{order.IdOrder}" : order.OrderCode,
                    PartnerName = string.IsNullOrWhiteSpace(order.CustomerName) ? "Khach hang" : order.CustomerName,
                    PartnerRole = "customer",
                    Status = order.Status,
                    LastTime = order.UpdatedAt ?? order.CreatedAt
                })
                .ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChatUnread()
        {
            if (!RequireRestaurant())
                return Json(new { count = 0 });

            var restaurant = await GetManagedRestaurantAsync();
            if (restaurant == null)
                return Json(new { count = 0 });

            var orders = await _apiClient.GetResultAsync<List<Order>>($"Orders/restaurant/{restaurant.IdRestaurant}") ?? new List<Order>();
            var unreadCounts = await Task.WhenAll(orders.Select(order =>
                _apiClient.GetResultAsync<int>($"Chat/rooms/order_{order.IdOrder}/unread?role=restaurant")));

            return Json(new { count = unreadCounts.Sum() });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status, int page = 1)
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

            return RedirectToAction("Orders", new { area = "Restaurant", page });
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder(int orderId, string address, string note, string status, int page = 1)
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

            return RedirectToAction("Orders", new { area = "Restaurant", page });
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId, int page = 1)
        {
            var client = GetApiClient();
            try
            {
                var request = new
                {
                    Status = "cancelled",
                    CancelReason = "Nha hang huy don"
                };
                var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"Orders/{orderId}/status", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Đã hủy đơn hàng. Đơn vẫn được lưu trong lịch sử.";
                }
                else
                {
                    TempData["Error"] = "Hủy đơn hàng thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("Orders", new { area = "Restaurant", page });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatusWithVerify(int orderId, string status, string verifyCode, int page = 1)
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
                            return RedirectToAction("Orders", new { area = "Restaurant", page });
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

            return RedirectToAction("Orders", new { area = "Restaurant", page });
        }

        [HttpPost]
        public async Task<IActionResult> SimulateDelivery(int orderId, int page = 1)
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

            return RedirectToAction("Orders", new { area = "Restaurant", page });
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

        public async Task<IActionResult> ExportRevenueReport()
        {
            if (!RequireRestaurant())
                return NotFound();

            var restaurant = await GetManagedRestaurantAsync();
            if (restaurant == null)
                return NotFound();

            var orders = await _apiClient.GetResultAsync<List<Order>>($"Orders/restaurant/{restaurant.IdRestaurant}") ?? new List<Order>();
            var dailyRevenue = orders
                .Where(o => o.Status != "canceled" && o.Status != "cancelled")
                .GroupBy(o => o.CreatedAt.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new RevenueReportRow
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Revenue = g.Sum(o => o.FinalTotal)
                })
                .ToList();

            return File(
                CreateRevenueReportPdf(
                    restaurant.NameRestaurant,
                    dailyRevenue,
                    orders.Count(o => o.Status is "canceled" or "cancelled"),
                    DateTime.Now),
                "application/pdf",
                $"bao-cao-doanh-thu-{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> Profile()
        {
            if (!RequireRestaurant())
                return NotFound();

            ViewData["Title"] = "Thông tin Nhà hàng";
            var restaurant = await GetManagedRestaurantAsync();

            ViewBag.Categories = await _apiClient.GetResultAsync<List<Category>>("Category", false) ?? new List<Category>();
            ViewBag.Foods = restaurant == null
                ? new List<Food>()
                : await _apiClient.GetResultAsync<List<Food>>($"Food/restaurant/{restaurant.IdRestaurant}", false) ?? new List<Food>();
            ViewBag.Promotions = restaurant == null
                ? new List<Promotion>()
                : await _apiClient.GetResultAsync<List<Promotion>>($"Promotion/restaurant/{restaurant.IdRestaurant}", false) ?? new List<Promotion>();
            ViewBag.Reviews = restaurant == null
                ? new List<ReviewViewModel>()
                : await _apiClient.GetResultAsync<List<ReviewViewModel>>($"Restaurants/{restaurant.IdRestaurant}/reviews", false) ?? new List<ReviewViewModel>();

            return View(restaurant ?? new RestaurantEntity());
        }

        [HttpPost]
        public async Task<IActionResult> CreateFood(int idCategory, string name, string description, IFormFile? imageFile, string video, decimal price, decimal? discount, int? prepTime, int dailyQuantity)
        {
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                var imageUrl = await UploadImageAsync(imageFile, "Upload/food")
                    ?? "/images/placeholder-food.svg";
                var request = new
                {
                    IdCategory = idCategory,
                    IdRestaurant = restaurantId,
                    Name = name,
                    Description = description ?? "",
                    Image = imageUrl,
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
        public async Task<IActionResult> UpdateFood(int idFood, int idCategory, string name, string description, string? currentImage, IFormFile? imageFile, string video, decimal price, decimal? discount, int? prepTime, int dailyQuantity)
        {
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                var imageUrl = await UploadImageAsync(imageFile, "Upload/food")
                    ?? (string.IsNullOrWhiteSpace(currentImage) ? "/images/placeholder-food.svg" : currentImage);
                var request = new
                {
                    IdCategory = idCategory,
                    IdRestaurant = restaurantId,
                    Name = name,
                    Description = description ?? "",
                    Image = imageUrl,
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
        public async Task<IActionResult> SaveSettings(string nameRestaurant, string description, string address, string? currentImage, IFormFile? imageFile, string openTime, string closeTime)
        {
            int restaurantId = GetRestaurantId();
            var client = GetApiClient();

            try
            {
                TimeSpan.TryParse(openTime, out var parsedOpen);
                TimeSpan.TryParse(closeTime, out var parsedClose);
                var imageUrl = await UploadImageAsync(imageFile, "Upload/restaurant")
                    ?? (string.IsNullOrWhiteSpace(currentImage) ? "/images/placeholder-restaurant.svg" : currentImage);

                var updateDto = new
                {
                    NameRestaurant = nameRestaurant,
                    Description = description,
                    Address = address,
                    Image = imageUrl,
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
        private async Task<string?> UploadImageAsync(IFormFile? imageFile, string endpoint)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            using var form = new MultipartFormDataContent();
            using var fileContent = new StreamContent(imageFile.OpenReadStream());
            if (!string.IsNullOrWhiteSpace(imageFile.ContentType))
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            form.Add(fileContent, "file", imageFile.FileName);

            var response = await GetApiClient().PostAsync(endpoint, form);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<string>>(content, _jsonOptions);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(result?.Message ?? "Không thể tải ảnh lên.");

            return result?.Results;
        }

        private HttpClient GetApiClient()
        {
            return _apiClient.CreateClient();
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
            return role == "restaurant";
        }

        private static byte[] CreateRevenueReportPdf(
            string restaurantName,
            IReadOnlyList<RevenueReportRow> rows,
            int cancelledOrders,
            DateTime generatedAt)
        {
            const int firstPageRows = 20;
            const int laterPageRows = 31;
            var pages = new List<List<RevenueReportRow>>();
            pages.Add(rows.Take(firstPageRows).ToList());
            for (var offset = firstPageRows; offset < rows.Count; offset += laterPageRows)
                pages.Add(rows.Skip(offset).Take(laterPageRows).ToList());

            var totalRevenue = rows.Sum(row => row.Revenue);
            var totalOrders = rows.Sum(row => row.Count);
            var averageOrder = totalOrders == 0 ? 0 : totalRevenue / totalOrders;
            var dateRange = rows.Count == 0
                ? "Chua co du lieu doanh thu"
                : $"{rows.Min(row => row.Date):dd/MM/yyyy} - {rows.Max(row => row.Date):dd/MM/yyyy}";

            var objects = new List<string>();
            var pageObjectIds = Enumerable.Range(0, pages.Count).Select(i => 5 + (i * 2)).ToList();
            objects.Add("<< /Type /Catalog /Pages 2 0 R >>");
            objects.Add($"<< /Type /Pages /Kids [{string.Join(" ", pageObjectIds.Select(id => $"{id} 0 R"))}] /Count {pages.Count} >>");
            objects.Add("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>");
            objects.Add("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold >>");

            for (var i = 0; i < pages.Count; i++)
            {
                var pageObjectId = pageObjectIds[i];
                var contentObjectId = pageObjectId + 1;
                objects.Add($"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 3 0 R /F2 4 0 R >> >> /Contents {contentObjectId} 0 R >>");

                var content = BuildRevenueReportPage(
                    i,
                    pages.Count,
                    pages[i],
                    restaurantName,
                    dateRange,
                    totalRevenue,
                    totalOrders,
                    averageOrder,
                    cancelledOrders,
                    generatedAt);
                objects.Add($"<< /Length {Encoding.ASCII.GetByteCount(content)} >>\nstream\n{content}\nendstream");
            }

            var pdf = new StringBuilder("%PDF-1.4\n");
            var offsets = new List<int> { 0 };
            for (var i = 0; i < objects.Count; i++)
            {
                offsets.Add(Encoding.ASCII.GetByteCount(pdf.ToString()));
                pdf.Append(i + 1).Append(" 0 obj\n").Append(objects[i]).Append("\nendobj\n");
            }

            var xrefOffset = Encoding.ASCII.GetByteCount(pdf.ToString());
            pdf.Append("xref\n0 ").Append(objects.Count + 1).Append("\n");
            pdf.Append("0000000000 65535 f \n");
            foreach (var offset in offsets.Skip(1))
                pdf.Append(offset.ToString("D10")).Append(" 00000 n \n");

            pdf.Append("trailer\n<< /Size ").Append(objects.Count + 1).Append(" /Root 1 0 R >>\n");
            pdf.Append("startxref\n").Append(xrefOffset).Append("\n%%EOF");
            return Encoding.ASCII.GetBytes(pdf.ToString());
        }

        private static string BuildRevenueReportPage(
            int pageIndex,
            int pageCount,
            IReadOnlyList<RevenueReportRow> rows,
            string restaurantName,
            string dateRange,
            decimal totalRevenue,
            int totalOrders,
            decimal averageOrder,
            int cancelledOrders,
            DateTime generatedAt)
        {
            var content = new StringBuilder();
            DrawRectangle(content, 0, 0, 595, 842, "1 1 1");
            DrawRectangle(content, 0, 742, 595, 100, "0.04 0.36 0.28");
            DrawRectangle(content, 42, 758, 5, 54, "0.18 0.83 0.60");
            DrawText(content, "F2", 23, 58, 792, "BAO CAO DOANH THU", "1 1 1");
            DrawText(content, "F1", 11, 58, 772, restaurantName, "0.88 0.97 0.94");
            DrawText(content, "F1", 9, 430, 796, $"Ngay xuat: {generatedAt:dd/MM/yyyy}", "1 1 1");
            DrawText(content, "F1", 9, 430, 779, $"Trang {pageIndex + 1} / {pageCount}", "1 1 1");

            var tableY = 692;
            if (pageIndex == 0)
            {
                DrawText(content, "F1", 10, 44, 724, $"Ky bao cao: {dateRange}  |  Khong bao gom don da huy");
                DrawMetricCard(content, 42, 654, 122, 54, "TONG DOANH THU", $"{totalRevenue:N0} VND", "0.93 0.98 0.95", "0.04 0.36 0.28");
                DrawMetricCard(content, 174, 654, 108, 54, "DON HOP LE", totalOrders.ToString("N0"), "0.93 0.96 1", "0.10 0.31 0.64");
                DrawMetricCard(content, 292, 654, 135, 54, "TB / DON", $"{averageOrder:N0} VND", "1 0.97 0.90", "0.72 0.42 0.04");
                DrawMetricCard(content, 437, 654, 116, 54, "DON DA HUY", cancelledOrders.ToString("N0"), "1 0.94 0.94", "0.75 0.14 0.14");
                tableY = 624;
            }
            else
            {
                DrawText(content, "F2", 13, 42, 716, "CHI TIET DOANH THU THEO NGAY (TIEP THEO)");
            }

            DrawRectangle(content, 42, tableY - 24, 511, 26, "0.04 0.36 0.28");
            DrawText(content, "F2", 10, 56, tableY - 15, "NGAY", "1 1 1");
            DrawText(content, "F2", 10, 234, tableY - 15, "SO DON", "1 1 1");
            DrawText(content, "F2", 10, 398, tableY - 15, "DOANH THU (VND)", "1 1 1");

            var rowY = tableY - 50;
            if (rows.Count == 0)
            {
                DrawRectangle(content, 42, rowY - 7, 511, 28, "0.97 0.98 0.98");
                DrawText(content, "F1", 10, 56, rowY + 3, "Chua co doanh thu trong ky bao cao.");
            }
            else
            {
                for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
                {
                    var row = rows[rowIndex];
                    if (rowIndex % 2 == 0)
                        DrawRectangle(content, 42, rowY - 7, 511, 28, "0.97 0.98 0.98");

                    DrawText(content, "F1", 10, 56, rowY + 3, row.Date.ToString("dd/MM/yyyy"));
                    DrawText(content, "F1", 10, 234, rowY + 3, row.Count.ToString("N0"));
                    DrawText(content, "F2", 10, 398, rowY + 3, row.Revenue.ToString("N0"));
                    DrawLine(content, 42, rowY - 8, 553, rowY - 8, "0.90 0.92 0.93");
                    rowY -= 29;
                }
            }

            DrawLine(content, 42, 42, 553, 42, "0.86 0.89 0.90");
            DrawText(content, "F1", 8, 42, 26, "Bao cao duoc tao tu he thong Mon Ngon Tai Nha");
            DrawText(content, "F1", 8, 474, 26, $"Trang {pageIndex + 1}/{pageCount}");
            return content.ToString();
        }

        private static void DrawMetricCard(StringBuilder content, int x, int y, int width, int height, string title, string value, string fillColor, string textColor)
        {
            DrawRectangle(content, x, y, width, height, fillColor);
            DrawText(content, "F1", 8, x + 10, y + height - 17, title, "0.38 0.43 0.48");
            DrawText(content, "F2", 12, x + 10, y + 15, value, textColor);
        }

        private static void DrawRectangle(StringBuilder content, int x, int y, int width, int height, string color)
        {
            content.Append(color).Append(" rg\n")
                .Append(x).Append(' ').Append(y).Append(' ').Append(width).Append(' ').Append(height).Append(" re f\n");
        }

        private static void DrawLine(StringBuilder content, int x1, int y1, int x2, int y2, string color)
        {
            content.Append(color).Append(" RG\n0.5 w\n")
                .Append(x1).Append(' ').Append(y1).Append(" m ")
                .Append(x2).Append(' ').Append(y2).Append(" l S\n");
        }

        private static void DrawText(StringBuilder content, string font, int size, int x, int y, string text, string color = "0.11 0.16 0.21")
        {
            content.Append(color).Append(" rg\nBT /").Append(font).Append(' ').Append(size)
                .Append(" Tf 1 0 0 1 ").Append(x).Append(' ').Append(y).Append(" Tm (")
                .Append(EscapePdfText(text)).Append(") Tj ET\n");
        }

        private static string EscapePdfText(string text)
        {
            var normalized = text
                .Replace('đ', 'd')
                .Replace('Đ', 'D')
                .Normalize(NormalizationForm.FormD);

            var ascii = new string(normalized
                .Where(character => CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark && character <= 127)
                .ToArray());

            return ascii.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }

        private sealed class RevenueReportRow
        {
            public DateTime Date { get; set; }
            public int Count { get; set; }
            public decimal Revenue { get; set; }
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
