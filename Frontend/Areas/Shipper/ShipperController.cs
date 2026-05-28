using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Areas.Shipper.Models;
using _225DAPM32.Models;
using _225DAPM32.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace _225DAPM32.Areas.Shipper
{
    [Area("Shipper")]
    public class ShipperController : Controller
    {
        private readonly ApiClient _apiClient;

        public ShipperController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HttpContext.Session.GetString("Role") != "shipper")
            {
                context.Result = NotFound();
                return;
            }

            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> Index()
        {
            var currentOrders = await GetCurrentOrdersAsync();
            var historyOrders = await GetHistoryOrdersAsync();
            ViewData["Title"] = "Dashboard Shipper";
            var model = new ShipperDashboardViewModel
            {
                ActiveDeliveries = currentOrders.Count,
                CompletedDeliveries = historyOrders.Count(o => o.Status == "completed"),
                TodayEarnings = historyOrders
                    .Where(o => o.Status == "completed" && (o.UpdatedAt ?? o.CreatedAt).Date == DateTime.Today)
                    .Sum(o => o.ShippingFee),
                Rating = 4.8m,
                CurrentOrders = currentOrders.OrderByDescending(o => o.CreatedAt).Take(5).ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> Orders()
        {
            ViewData["Title"] = "Đơn hàng cần giao";
            ViewBag.AvailableOrders = await GetAvailableOrdersAsync();
            return View(await GetCurrentOrdersAsync());
        }

        public async Task<IActionResult> History()
        {
            ViewData["Title"] = "Lịch sử giao hàng";
            return View(await GetHistoryOrdersAsync());
        }

        public async Task<IActionResult> Earnings()
        {
            var currentOrders = await GetCurrentOrdersAsync();
            var historyOrders = await GetHistoryOrdersAsync();
            var completedOrders = historyOrders.Where(o => o.Status == "completed").ToList();
            ViewData["Title"] = "Thu nhập";
            var earnings = new ShipperEarningsViewModel
            {
                WeekTotal = completedOrders
                    .Where(o => (o.UpdatedAt ?? o.CreatedAt) >= DateTime.Today.AddDays(-6))
                    .Sum(o => o.ShippingFee),
                TodayTotal = completedOrders
                    .Where(o => (o.UpdatedAt ?? o.CreatedAt).Date == DateTime.Today)
                    .Sum(o => o.ShippingFee),
                CompletedOrders = completedOrders.Count,
                PendingOrders = currentOrders.Count
            };
            return View(earnings);
        }

        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var user = int.TryParse(userId, out var id)
                ? await _apiClient.GetResultAsync<User>($"Users/{id}")
                : null;
            var historyOrders = await GetHistoryOrdersAsync();
            ViewData["Title"] = "Thông tin cá nhân";
            var profile = new ShipperProfileViewModel
            {
                Name = user?.FullName ?? "Shipper",
                Phone = user?.Phone ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                Status = "Online",
                Rating = 4.8m,
                CompletedDeliveries = historyOrders.Count(o => o.Status == "completed")
            };
            return View(profile);
        }

        public IActionResult Settings()
        {
            ViewData["Title"] = "Cài đặt";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDelivering(int orderId)
        {
            var (success, _, message) = await _apiClient.PutResultAsync<Order>(
                $"Shipper/orders/{orderId}/delivering",
                new { Status = "delivering" });

            TempData[success ? "Success" : "Error"] = success
                ? "Đã bắt đầu giao đơn hàng."
                : (message ?? "Không thể cập nhật đơn hàng.");

            return RedirectToAction("Orders", new { area = "Shipper" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            var (success, _, message) = await _apiClient.PutResultAsync<Order>(
                $"Shipper/orders/{orderId}/accept",
                new { });

            TempData[success ? "Success" : "Error"] = success
                ? "Bạn đã nhận đơn. Nhà hàng đã được thông báo để chuẩn bị món."
                : (message ?? "Không thể nhận đơn hàng.");

            return RedirectToAction("Orders", new { area = "Shipper" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPickedUp(int orderId)
        {
            var (success, _, message) = await _apiClient.PutResultAsync<Order>(
                $"Shipper/orders/{orderId}/delivering",
                new { Status = "delivering" });

            TempData[success ? "Success" : "Error"] = success
                ? "Đã xác nhận lấy hàng. Bắt đầu giao đến khách."
                : (message ?? "Không thể xác nhận lấy hàng.");

            return RedirectToAction("Orders", new { area = "Shipper" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCompleted(int orderId)
        {
            var (success, _, message) = await _apiClient.PutResultAsync<Order>(
                $"Shipper/orders/{orderId}/completed",
                new { Status = "completed" });

            TempData[success ? "Success" : "Error"] = success
                ? "Đã xác nhận giao hàng thành công."
                : (message ?? "Không thể hoàn tất đơn hàng.");

            return RedirectToAction("Orders", new { area = "Shipper" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLocation(string latitude, string longitude)
        {
            if (!decimal.TryParse(latitude, NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) ||
                !decimal.TryParse(longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out var lng))
            {
                return BadRequest(new { success = false, message = "Tọa độ không hợp lệ." });
            }

            var (success, _, message) = await _apiClient.PutResultAsync<bool>(
                "Shipper/location",
                new { Latitude = lat, Longitude = lng });

            return Json(new { success, message });
        }

        private async Task<List<Order>> GetCurrentOrdersAsync()
        {
            return await _apiClient.GetResultAsync<List<Order>>("Shipper/orders") ?? new List<Order>();
        }

        private async Task<List<Order>> GetAvailableOrdersAsync()
        {
            return await _apiClient.GetResultAsync<List<Order>>("Shipper/available-orders") ?? new List<Order>();
        }

        private async Task<List<Order>> GetHistoryOrdersAsync()
        {
            return await _apiClient.GetResultAsync<List<Order>>("Shipper/history") ?? new List<Order>();
        }
    }
}
