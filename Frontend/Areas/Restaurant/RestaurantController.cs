using Microsoft.AspNetCore.Mvc;
using System.Linq;
using _225DAPM32.Areas.Restaurant.Models;
using _225DAPM32.Models;
using RestaurantEntity = _225DAPM32.Models.Restaurant;
using CategoryEntity = _225DAPM32.Models.Category;

namespace _225DAPM32.Areas.Restaurant
{
    [Area("Restaurant")]
    public class RestaurantController : Controller
    {
        private static readonly RestaurantEntity SampleRestaurant = new()
        {
            Id_Restaurant = 1,
            Name_Restaurant = "Nhà hàng Phở 24h",
            Description = "Phở gà, phở bò và món ăn Việt Nam chuẩn vị.",
            Image = "/images/restaurant1.jpg",
            Address = "123 Đường ABC, Q.1, TP.HCM",
            OpenTime = new TimeSpan(6, 0, 0),
            CloseTime = new TimeSpan(22, 0, 0),
            Lat = 10.762622m,
            Lng = 106.660172m
        };

        private static readonly CategoryEntity PhoCategory = new() { Id_Category = 1, Name = "Phở", Icon = "fas fa-bowl-food" };
        private static readonly CategoryEntity AppetizerCategory = new() { Id_Category = 2, Name = "Món khai vị", Icon = "fas fa-leaf" };

        private static readonly List<Food> SampleMenu = new()
        {
            new Food { Id_Food = 1, Id_Category = PhoCategory.Id_Category, Id_Restaurant = SampleRestaurant.Id_Restaurant, Name = "Phở bò đặc biệt", Description = "Phở bò ngon với nước dùng thơm", Image = "/images/food1.jpg", Video = "", Price = 79000m, Discount = 0m, Cook_Count = 120, Prep_Time = 15, Category = PhoCategory, Restaurant = SampleRestaurant },
            new Food { Id_Food = 2, Id_Category = PhoCategory.Id_Category, Id_Restaurant = SampleRestaurant.Id_Restaurant, Name = "Phở gà", Description = "Phở gà mềm mại với nước dùng thanh", Image = "/images/food2.jpg", Video = "", Price = 68000m, Discount = 0m, Cook_Count = 90, Prep_Time = 12, Category = PhoCategory, Restaurant = SampleRestaurant },
            new Food { Id_Food = 3, Id_Category = AppetizerCategory.Id_Category, Id_Restaurant = SampleRestaurant.Id_Restaurant, Name = "Gỏi cuốn", Description = "Gỏi cuốn tôm thịt tươi ngon", Image = "/images/food3.jpg", Video = "", Price = 55000m, Discount = 0m, Cook_Count = 75, Prep_Time = 10, Category = AppetizerCategory, Restaurant = SampleRestaurant }
        };

        private static readonly List<Order> SampleOrders = new()
        {
            new Order { Id_Order = 2001, Id_User = 1, Total = 185000m, ShippingFee = 15000m, Discount = 0m, FinalTotal = 200000m, Status = "Preparing", Created_At = DateTime.Now.AddMinutes(-30), EstimatedDelivery = DateTime.Now.AddMinutes(25), DriverName = "Nguyễn Văn C" },
            new Order { Id_Order = 2002, Id_User = 2, Total = 98000m, ShippingFee = 15000m, Discount = 0m, FinalTotal = 113000m, Status = "Confirmed", Created_At = DateTime.Now.AddMinutes(-15), EstimatedDelivery = DateTime.Now.AddMinutes(40), DriverName = "Lê Thị D" }
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard Nhà hàng";
            var model = new RestaurantDashboardViewModel
            {
                ActiveOrders = SampleOrders.Count(o => o.Status != "Completed"),
                RevenueToday = SampleOrders.Sum(o => o.FinalTotal),
                MenuItems = SampleMenu.Count,
                Rating = 4.7m
            };
            return View(model);
        }

        public IActionResult Menu()
        {
            ViewData["Title"] = "Quản lý Menu";
            return View(SampleMenu);
        }

        public IActionResult Orders()
        {
            ViewData["Title"] = "Đơn hàng";
            return View(SampleOrders);
        }

        public IActionResult Analytics()
        {
            ViewData["Title"] = "Thống kê";
            return View();
        }

        public IActionResult Profile()
        {
            ViewData["Title"] = "Thông tin Nhà hàng";
            return View(SampleRestaurant);
        }

        public IActionResult Settings()
        {
            ViewData["Title"] = "Cài đặt";
            return View();
        }
    }
}