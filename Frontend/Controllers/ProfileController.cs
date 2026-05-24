using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using _225DAPM32.Services;

namespace _225DAPM32.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApiClient _apiClient;

        public ProfileController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            ViewBag.User = await GetCurrentUserAsync();
            ViewBag.Orders = await GetMyOrdersAsync();
            ViewBag.Addresses = new List<Address>();
            return View();
        }

        public async Task<IActionResult> Orders()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            ViewBag.User = await GetCurrentUserAsync();
            ViewBag.Orders = await GetMyOrdersAsync();
            return View();
        }

        public async Task<IActionResult> Addresses()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            ViewBag.User = await GetCurrentUserAsync();
            ViewBag.Addresses = new List<Address>();
            return View();
        }

        public async Task<IActionResult> Settings()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            ViewBag.User = await GetCurrentUserAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateUserDto dto)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var userId = HttpContext.Session.GetString("UserId");
            if (!int.TryParse(userId, out var id))
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var (success, user, message) = await _apiClient.PutResultAsync<User>($"Users/{id}", dto);
            TempData[success ? "Success" : "Error"] = success
                ? "Cập nhật thông tin thành công."
                : (message ?? "Không thể cập nhật thông tin.");

            if (success && user != null && !string.IsNullOrWhiteSpace(user.Username))
                HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Settings", "Profile", new { area = "" });
        }

        public async Task<IActionResult> OrderTracking(int id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var order = await _apiClient.GetResultAsync<Order>($"Orders/{id}");
            if (order == null)
                return NotFound();

            ViewBag.User = await GetCurrentUserAsync();
            return View(order);
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            return int.TryParse(userId, out var id)
                ? await _apiClient.GetResultAsync<User>($"Users/{id}")
                : null;
        }

        private async Task<List<Order>> GetMyOrdersAsync()
        {
            return await _apiClient.GetResultAsync<List<Order>>("Orders") ?? new List<Order>();
        }

        private bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Token"));
        }
    }
}
