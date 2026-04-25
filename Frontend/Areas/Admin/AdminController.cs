using Microsoft.AspNetCore.Mvc;
using System.Linq;
using _225DAPM32.Areas.Admin.Models;
using _225DAPM32.Models;
using RestaurantEntity = _225DAPM32.Models.Restaurant;

namespace _225DAPM32.Areas.Admin
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private static readonly List<RestaurantEntity> SampleRestaurants = new()
        {
            new RestaurantEntity { Id_Restaurant = 1, Name_Restaurant = "Nhà hàng Phở 24h", Description = "Phở gà, phở bò đặc biệt", Image = "/images/restaurant1.jpg", Address = "123 Đường ABC, Q.1", OpenTime = new TimeSpan(6,0,0), CloseTime = new TimeSpan(22,0,0), Lat = 10.762622m, Lng = 106.660172m },
            new RestaurantEntity { Id_Restaurant = 2, Name_Restaurant = "Gà rán Crispy", Description = "Gà rán giòn tan", Image = "/images/restaurant2.jpg", Address = "456 Đường XYZ, Q.3", OpenTime = new TimeSpan(10,0,0), CloseTime = new TimeSpan(23,0,0), Lat = 10.762700m, Lng = 106.682000m }
        };

        private static readonly List<User> SampleUsers = new()
        {
            new User { Id_User = 1, Username = "nguyenvana", Email = "nguyenvana@example.com", Phone = "0912345678", Status = "Active", Created_At = DateTime.Now.AddMonths(-5), LastOnline = DateTime.Now.AddMinutes(-12), Avatar = "/images/user1.jpg" },
            new User { Id_User = 2, Username = "tranthib", Email = "tranthib@example.com", Phone = "0987654321", Status = "Active", Created_At = DateTime.Now.AddMonths(-2), LastOnline = DateTime.Now.AddHours(-1), Avatar = "/images/user2.jpg" }
        };

        private static readonly List<Order> SampleOrders = new()
        {
            new Order { Id_Order = 1001, Id_User = 1, Total = 185000m, ShippingFee = 15000m, Discount = 0m, FinalTotal = 200000m, Status = "Delivering", Created_At = DateTime.Now.AddMinutes(-40), DriverName = "Trần Văn C", DriverPhone = "0911222333", EstimatedDelivery = DateTime.Now.AddMinutes(20) },
            new Order { Id_Order = 1002, Id_User = 2, Total = 255000m, ShippingFee = 15000m, Discount = 0m, FinalTotal = 270000m, Status = "Completed", Created_At = DateTime.Now.AddHours(-2), DriverName = "Lê Thị D", DriverPhone = "0909988776", EstimatedDelivery = DateTime.Now.AddHours(-1) }
        };

        private static readonly List<SecurityPolicyViewModel> SamplePolicies = new()
        {
            new SecurityPolicyViewModel { Id = 1, Title = "Xác thực 2 yếu tố", Description = "Yêu cầu tất cả admin bật xác thực hai yếu tố cho tài khoản.", EffectiveDate = DateTime.Now.AddMonths(-3), IsActive = true },
            new SecurityPolicyViewModel { Id = 2, Title = "Mật khẩu mạnh", Description = "Bắt buộc mật khẩu phải có ít nhất 12 ký tự, chữ hoa, chữ thường và số.", EffectiveDate = DateTime.Now.AddMonths(-6), IsActive = true },
            new SecurityPolicyViewModel { Id = 3, Title = "Đăng xuất tự động", Description = "Tự động đăng xuất sau 15 phút không hoạt động.", EffectiveDate = DateTime.Now.AddMonths(-4), IsActive = true }
        };

        private static readonly List<ViolationAlert> ViolationAlerts = new()
        {
            new ViolationAlert { Id = 1, Title = "Đăng nhập khả nghi", Description = "Phát hiện đăng nhập từ IP mới tại Hà Nội.", Severity = "High", CreatedAt = DateTime.Now.AddHours(-1), Status = "Pending" },
            new ViolationAlert { Id = 2, Title = "Thay đổi chính sách chưa xác thực", Description = "Cố gắng truy cập trang quản lý chính sách từ tài khoản chưa xác thực.", Severity = "Medium", CreatedAt = DateTime.Now.AddHours(-3), Status = "Reviewed" },
            new ViolationAlert { Id = 3, Title = "Thực thi API bị từ chối", Description = "API quản lý người dùng trả lỗi 403 khi truy xuất từ IP lạ.", Severity = "High", CreatedAt = DateTime.Now.AddDays(-1), Status = "Pending" }
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Admin Dashboard";
            var model = new AdminDashboardViewModel
            {
                RestaurantsCount = SampleRestaurants.Count,
                UsersCount = SampleUsers.Count,
                OrdersToday = SampleOrders.Count,
                RevenueToday = SampleOrders.Sum(o => o.FinalTotal)
            };
            return View(model);
        }

        public IActionResult Restaurants()
        {
            ViewData["Title"] = "Quản lý Nhà hàng";
            return View(SampleRestaurants);
        }

        public IActionResult Users()
        {
            ViewData["Title"] = "Quản lý Người dùng";
            return View(SampleUsers);
        }

        public IActionResult Orders()
        {
            ViewData["Title"] = "Quản lý Đơn hàng";
            return View(SampleOrders);
        }

        public IActionResult Policies()
        {
            ViewData["Title"] = "Chính sách bảo mật";
            return View(SamplePolicies);
        }

        public IActionResult Violations()
        {
            ViewData["Title"] = "Cảnh báo vi phạm";
            return View(ViolationAlerts);
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
    }
}