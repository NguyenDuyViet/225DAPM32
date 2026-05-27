using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using _225DAPM32.Services;

namespace _225DAPM32.Controllers
{
    public class CartController : Controller
    {
        private readonly ApiClient _apiClient;

        public CartController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var cart = await _apiClient.GetResultAsync<CartResponse>("Cart") ?? new CartResponse();
            ViewBag.User = await GetCurrentUserAsync();
            ViewBag.Vouchers = await _apiClient.GetResultAsync<List<Voucher>>("Cart/vouchers") ?? new List<Voucher>();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int foodId, int quantity = 1, string? note = null, string? returnUrl = null)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var (success, _, message) = await _apiClient.PostResultAsync<CartResponse>("Cart/items", new CartItemRequest
            {
                IdFood = foodId,
                Quantity = Math.Max(1, quantity),
                Note = note
            });

            TempData[success ? "Success" : "Error"] = success ? "Đã thêm món vào giỏ hàng." : (message ?? "Không thể thêm món vào giỏ hàng.");

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Cart", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(int idCartFood, int quantity, string? note)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var (success, _, message) = await _apiClient.PutResultAsync<CartResponse>($"Cart/items/{idCartFood}", new UpdateCartItemRequest
            {
                Quantity = Math.Max(1, quantity),
                Note = note
            });

            TempData[success ? "Success" : "Error"] = success ? "Đã cập nhật giỏ hàng." : (message ?? "Không thể cập nhật giỏ hàng.");
            return RedirectToAction("Index", "Cart", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int idCartFood)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var success = await _apiClient.DeleteAsync($"Cart/items/{idCartFood}");
            TempData[success ? "Success" : "Error"] = success ? "Đã xóa món khỏi giỏ hàng." : "Không thể xóa món khỏi giỏ hàng.";
            return RedirectToAction("Index", "Cart", new { area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string deliveryAddress, string paymentMethod = "cash", int? idVoucher = null, string? note = null)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Home", new { area = "", loginRequired = true });

            var (success, order, message) = await _apiClient.PostResultAsync<Order>("Orders/checkout", new CreateOrderRequest
            {
                DeliveryAddress = deliveryAddress,
                PaymentMethod = paymentMethod,
                IdVoucher = idVoucher,
                ShippingFee = 15000m,
                Note = note
            });

            if (!success || order == null)
            {
                TempData["Error"] = message ?? "Không thể đặt hàng. Vui lòng kiểm tra giỏ hàng.";
                return RedirectToAction("Index", "Cart", new { area = "" });
            }

            TempData["Success"] = $"Đặt hàng thành công! Mã đơn hàng: {order.OrderCode}";
            return RedirectToAction("Orders", "Profile", new { area = "" });
        }

        private bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("Token"));
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            return int.TryParse(userId, out var id)
                ? await _apiClient.GetResultAsync<User>($"Users/{id}")
                : null;
        }
    }
}
