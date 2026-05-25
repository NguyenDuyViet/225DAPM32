using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Areas.Admin.Models;
using _225DAPM32.Models;
using _225DAPM32.Services;
using RestaurantEntity = _225DAPM32.Models.Restaurant;

namespace _225DAPM32.Areas.Admin
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ApiClient _apiClient;

        public AdminController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            if (!RequireAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var restaurants = await GetRestaurantsAsync();
            var users = await _apiClient.GetResultAsync<List<User>>("Users") ?? new List<User>();
            var orders = await _apiClient.GetResultAsync<List<Order>>("Orders/admin") ?? new List<Order>();
            var today = DateTime.Today;
            var past7Days = Enumerable.Range(0, 7).Select(offset => today.AddDays(offset - 6)).ToList();

            ViewData["Title"] = "Admin Dashboard";
            return View(new AdminDashboardViewModel
            {
                RestaurantsCount = restaurants.Count,
                UsersCount = users.Count,
                OrdersToday = orders.Count(o => o.CreatedAt.Date == today),
                RevenueToday = orders
                    .Where(o => o.CreatedAt.Date == today && o.Status != "canceled" && o.Status != "cancelled")
                    .Sum(o => o.FinalTotal),
                RecentOrders = orders.OrderByDescending(o => o.CreatedAt).Take(5).ToList(),
                RecentRestaurants = restaurants.OrderByDescending(r => r.IdRestaurant).Take(5).ToList(),
                RevenueLabels = past7Days.Select(day => day.ToString("dd/MM")).ToList(),
                RevenueData = past7Days.Select(day => orders
                    .Where(o => o.CreatedAt.Date == day && o.Status != "canceled" && o.Status != "cancelled")
                    .Sum(o => o.FinalTotal)).ToList(),
                StatusLabels = new List<string> { "Hoàn thành", "Đang giao", "Đã xác nhận", "Chờ xử lý", "Đã hủy" },
                StatusData = new List<int>
                {
                    orders.Count(o => o.Status == "completed"),
                    orders.Count(o => o.Status == "delivering"),
                    orders.Count(o => o.Status == "confirmed"),
                    orders.Count(o => o.Status == "pending"),
                    orders.Count(o => o.Status == "canceled" || o.Status == "cancelled")
                }
            });
        }

        public async Task<IActionResult> Restaurants()
        {
            if (!RequireAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            ViewData["Title"] = "Quản lý Nhà hàng";
            return View(await GetRestaurantsAsync());
        }

        public async Task<IActionResult> Users()
        {
            if (!RequireAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            ViewData["Title"] = "Quản lý Người dùng";
            return View(await _apiClient.GetResultAsync<List<User>>("Users") ?? new List<User>());
        }

        public async Task<IActionResult> Orders()
        {
            if (!RequireAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            ViewData["Title"] = "Quản lý Đơn hàng";
            return View(await _apiClient.GetResultAsync<List<Order>>("Orders/admin") ?? new List<Order>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            if (!RequireAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var (success, _, message) = await _apiClient.PostResultAsync<User>($"Users/{id}/toggle-status", new { });
            TempData[success ? "Success" : "Error"] = message ?? (success
                ? "Đã cập nhật trạng thái tài khoản."
                : "Không thể cập nhật trạng thái tài khoản.");
            return RedirectToAction(nameof(Users));
        }

        public IActionResult Policies()
        {
            ViewData["Title"] = "Chính sách bảo mật";
            return View(new List<SecurityPolicyViewModel>());
        }

        public IActionResult Violations()
        {
            ViewData["Title"] = "Cảnh báo vi phạm";
            return View(new List<ViolationAlert>());
        }

        public IActionResult Reports()
        {
            ViewData["Title"] = "Báo cáo & Thống kê";
            return View();
        }

        public IActionResult Settings()
        {
            ViewData["Title"] = "Cài đặt Hệ thống";
            return View();
        }

        private async Task<List<RestaurantEntity>> GetRestaurantsAsync()
        {
            return await _apiClient.GetResultAsync<List<RestaurantEntity>>("Restaurants/all", false) ?? new List<RestaurantEntity>();
        }

        private bool RequireAdmin()
        {
            return HttpContext.Session.GetString("Role") == "admin";
        }
    }
}
