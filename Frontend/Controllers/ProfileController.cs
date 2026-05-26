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

        public async Task<IActionResult> Orders(int page = 1)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            const int pageSize = 5;
            var orders = await GetMyOrdersAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(orders.Count / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            ViewBag.User = await GetCurrentUserAsync();
            ViewBag.Orders = orders
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalOrders = orders.Count;
            return View();
        }

        public async Task<IActionResult> Chat(int? orderId)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var user = await GetCurrentUserAsync();
            var orders = (await GetMyOrdersAsync())
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            Order? selectedOrder = null;
            if (orderId.HasValue)
            {
                selectedOrder = orders.FirstOrDefault(o => o.IdOrder == orderId.Value);
                if (selectedOrder == null)
                    return NotFound();
            }
            else
            {
                foreach (var order in orders)
                {
                    var unreadCount = await _apiClient.GetResultAsync<int>(
                        $"Chat/rooms/order_{order.IdOrder}/unread?role=customer");
                    if (unreadCount > 0)
                    {
                        selectedOrder = order;
                        break;
                    }
                }

                selectedOrder ??= orders.FirstOrDefault();
            }

            ViewBag.User = user;
            ViewBag.Orders = orders;
            return View(selectedOrder);
        }

        [HttpGet]
        public async Task<IActionResult> ChatUnread()
        {
            if (!IsLoggedIn())
                return Json(new { count = 0 });

            var orders = await GetMyOrdersAsync();
            var unreadCounts = await Task.WhenAll(orders.Select(order =>
                _apiClient.GetResultAsync<int>($"Chat/rooms/order_{order.IdOrder}/unread?role=customer")));

            return Json(new { count = unreadCounts.Sum() });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int idOrder, int page = 1)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var (success, _, message) = await _apiClient.PutResultAsync<Order>(
                $"Orders/{idOrder}/cancel",
                new { CancelReason = "Khách hàng hủy đơn" });

            TempData[success ? "Success" : "Error"] = success
                ? "Đã hủy đơn hàng. Đơn hàng vẫn được lưu trong danh sách của bạn."
                : (message ?? "Không thể hủy đơn hàng.");

            return RedirectToAction(nameof(Orders), new { page });
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
