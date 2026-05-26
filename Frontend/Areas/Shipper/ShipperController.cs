using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Areas.Shipper.Models;
using _225DAPM32.Models;
using _225DAPM32.Services;

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
                Rating = 4.8m
            };
            return View(model);
        }

        public async Task<IActionResult> Orders()
        {
            ViewData["Title"] = "Đơn hàng cần giao";
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

        private async Task<List<Order>> GetCurrentOrdersAsync()
        {
            return await _apiClient.GetResultAsync<List<Order>>("Shipper/orders") ?? new List<Order>();
        }

        private async Task<List<Order>> GetHistoryOrdersAsync()
        {
            return await _apiClient.GetResultAsync<List<Order>>("Shipper/history") ?? new List<Order>();
        }
    }
}
