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

            ViewData["Title"] = "Admin Dashboard";
            return View(new AdminDashboardViewModel
            {
                RestaurantsCount = restaurants.Count,
                UsersCount = users.Count,
                OrdersToday = orders.Count(o => o.CreatedAt.Date == DateTime.Today),
                RevenueToday = orders.Where(o => o.CreatedAt.Date == DateTime.Today).Sum(o => o.FinalTotal)
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
