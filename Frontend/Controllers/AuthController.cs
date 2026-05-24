using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using _225DAPM32.Services;

namespace _225DAPM32.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiClient _apiClient;

        public AuthController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            var (success, token, message) = await _apiClient.PostResultAsync<string>("Auth/login", new LoginRequest
            {
                Username = username,
                Password = password
            }, false);

            if (!success || string.IsNullOrWhiteSpace(token))
            {
                TempData["Error"] = message ?? "Tên đăng nhập hoặc mật khẩu không đúng.";
                return RedirectToLocal(returnUrl);
            }

            SaveSession(token, username);
            return RedirectByRole(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string? fullName, string? phone)
        {
            var (created, _, message) = await _apiClient.PostResultAsync<User>("Users", new CreateUserDto
            {
                Username = username,
                Email = email,
                Password = password,
                FullName = fullName,
                Phone = phone
            }, false);

            if (!created)
            {
                TempData["Error"] = message ?? "Không thể đăng ký tài khoản.";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            TempData["Success"] = "Đăng ký thành công. Bạn đã được đăng nhập.";
            TempData["Success"] = "Đăng ký thành công. Bạn đã được đăng nhập.";
            TempData["Info"] = "Vui lòng cập nhật vị trí giao hàng trong phần cài đặt trước khi đặt món.";
            return await Login(username, password, Url.Action("Settings", "Profile", new { area = "" }));
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private void SaveSession(string token, string fallbackUsername)
        {
            var payload = ReadJwtPayload(token);
            HttpContext.Session.SetString("Token", token);
            HttpContext.Session.SetString("Username", payload.TryGetValue("sub", out var username) ? username : fallbackUsername);

            if (payload.TryGetValue("UserId", out var userId))
                HttpContext.Session.SetString("UserId", userId);

            var role = payload.TryGetValue("role", out var shortRole)
                ? shortRole
                : payload.GetValueOrDefault("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            if (!string.IsNullOrWhiteSpace(role))
                HttpContext.Session.SetString("Role", role);
        }

        private IActionResult RedirectByRole(string? returnUrl)
        {
            var roleRedirect = HttpContext.Session.GetString("Role") switch
            {
                "admin" => RedirectToAction("Index", "Admin", new { area = "Admin" }),
                "restaurant" => RedirectToAction("Index", "Restaurant", new { area = "Restaurant" }),
                _ => null
            };

            if (roleRedirect != null)
                return roleRedirect;

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Profile", new { area = "" });
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction("Index", "Home", new { area = "" });
        }

        private static Dictionary<string, string> ReadJwtPayload(string token)
        {
            var parts = token.Split('.');
            if (parts.Length < 2)
                return new Dictionary<string, string>();

            var payload = parts[1].Replace('-', '+').Replace('_', '/');
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));

            return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)?
                .ToDictionary(pair => pair.Key, pair => pair.Value.ToString())
                ?? new Dictionary<string, string>();
        }
    }
}
