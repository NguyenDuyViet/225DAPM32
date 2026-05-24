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
<<<<<<< HEAD
        private static readonly List<RestaurantEntity> SampleRestaurants = new()
        {
            new RestaurantEntity { IdRestaurant = 1, NameRestaurant = "Nhà hàng Phở 24h", Description = "Phở gà, phở bò đặc biệt", Image = "/images/restaurant1.jpg", Address = "123 Đường ABC, Q.1", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
            new RestaurantEntity { IdRestaurant = 2, NameRestaurant = "Gà rán Crispy", Description = "Gà rán giòn tan", Image = "/images/restaurant2.jpg", Address = "456 Đường XYZ, Q.3", OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(23, 0, 0), Lat = 10.762700m, Lng = 106.682000m }
        };
=======
        private readonly ApiClient _apiClient;
>>>>>>> 0046400d0711806a8ed86b03458f2aae1128e39b

        public AdminController(ApiClient apiClient)
        {
<<<<<<< HEAD
            new User { IdUser = 1, IdRole = 2, Username = "nguyenvana", Password = "********", FullName = "Nguyễn Văn A", Email = "nguyenvana@example.com", Phone = "0912345678", Address = "12 Nguyễn Trãi, Quận 1, TP.HCM", Avatar = "/images/user1.jpg", Status = "active", CreatedAt = DateTime.Now.AddMonths(-5), LastOnline = DateTime.Now.AddMinutes(-12), CurrentLat = 10.762622m, CurrentLng = 106.660172m, CancelRate = 0.02f, UpdateBio = "Khách hàng thân thiết" },
            new User { IdUser = 2, IdRole = 2, Username = "tranthib", Password = "********", FullName = "Trần Thị B", Email = "tranthib@example.com", Phone = "0987654321", Address = "45 Lê Lợi, Quận 3, TP.HCM", Avatar = "/images/user2.jpg", Status = "active", CreatedAt = DateTime.Now.AddMonths(-2), LastOnline = DateTime.Now.AddHours(-1), CurrentLat = 10.762700m, CurrentLng = 106.682000m, CancelRate = 0.01f, UpdateBio = "Thường đặt món buổi trưa" }
        };
=======
            _apiClient = apiClient;
        }
>>>>>>> 0046400d0711806a8ed86b03458f2aae1128e39b

        public async Task<IActionResult> Index()
        {
<<<<<<< HEAD
            new Order { IdOrder = 1001, IdUser = 1, IdRestaurant = 1, IdDriver = 6, IdVoucher = null, OrderCode = "MN1001", DeliveryAddress = "12 Nguyễn Trãi, Quận 1, TP.HCM", DeliveryLat = 10.762622m, DeliveryLng = 106.660172m, FoodAmount = 185000m, Total = 185000m, ShippingFee = 15000m, Discount = 0m, FinalTotal = 200000m, PaymentMethod = "COD", PaymentStatus = "unpaid", Status = "delivering", Note = "Giao giờ hành chính", CancelReason = null, CreatedAt = DateTime.Now.AddMinutes(-40), UpdatedAt = DateTime.Now.AddMinutes(-10), DriverName = "Trần Văn C", DriverPhone = "0911222333", EstimatedDelivery = DateTime.Now.AddMinutes(20) },
            new Order { IdOrder = 1002, IdUser = 2, IdRestaurant = 2, IdDriver = 7, IdVoucher = 1, OrderCode = "MN1002", DeliveryAddress = "45 Lê Lợi, Quận 3, TP.HCM", DeliveryLat = 10.762700m, DeliveryLng = 106.682000m, FoodAmount = 255000m, Total = 255000m, ShippingFee = 15000m, Discount = 0m, FinalTotal = 270000m, PaymentMethod = "Ví điện tử", PaymentStatus = "paid", Status = "completed", Note = "Không gọi trước", CancelReason = null, CreatedAt = DateTime.Now.AddHours(-2), UpdatedAt = DateTime.Now.AddHours(-1), DriverName = "Lê Thị D", DriverPhone = "0909988776", EstimatedDelivery = DateTime.Now.AddHours(-1) }
        };
=======
            if (!RequireAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });
>>>>>>> 0046400d0711806a8ed86b03458f2aae1128e39b

            var restaurants = await GetRestaurantsAsync();
            var users = await _apiClient.GetResultAsync<List<User>>("Users") ?? new List<User>();
            var orders = await _apiClient.GetResultAsync<List<Order>>("Orders/admin") ?? new List<Order>();

<<<<<<< HEAD
        private static readonly List<ViolationAlert> ViolationAlerts = new()
        {
            new ViolationAlert { Id = 1, Title = "Đăng nhập khả nghi", Description = "Phát hiện đăng nhập từ IP mới tại Hà Nội.", Severity = "High", CreatedAt = DateTime.Now.AddHours(-1), Status = "Pending" },
            new ViolationAlert { Id = 2, Title = "Thay đổi chính sách chưa xác thực", Description = "Cố gắng truy cập trang quản lý chính sách từ tài khoản chưa xác thực.", Severity = "Medium", CreatedAt = DateTime.Now.AddHours(-3), Status = "Reviewed" },
            new ViolationAlert { Id = 3, Title = "Thực thi API bị từ chối", Description = "API quản lý người dùng trả lỗi 403 khi truy xuất từ IP lạ.", Severity = "High", CreatedAt = DateTime.Now.AddDays(-1), Status = "Pending" }
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Tổng quan Admin";
            var model = new AdminDashboardViewModel
=======
            ViewData["Title"] = "Admin Dashboard";
            return View(new AdminDashboardViewModel
>>>>>>> 0046400d0711806a8ed86b03458f2aae1128e39b
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

<<<<<<< HEAD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleUserStatus(int id)
        {
            var user = SampleUsers.FirstOrDefault(item => item.IdUser == id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản cần cập nhật.";
                return RedirectToAction(nameof(Users));
            }

            user.Status = string.Equals(user.Status, "active", StringComparison.OrdinalIgnoreCase)
                ? "locked"
                : "active";
            TempData["Success"] = $"Đã cập nhật trạng thái tài khoản {user.Username}.";
            return RedirectToAction(nameof(Users));
        }

        public IActionResult Orders()
=======
        public async Task<IActionResult> Orders()
>>>>>>> 0046400d0711806a8ed86b03458f2aae1128e39b
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
