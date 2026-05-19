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
            IdRestaurant = 1,
            NameRestaurant = "Nhà hàng Phở 24h",
            Description = "Phở gà, phở bò và món ăn Việt Nam chuẩn vị.",
            Image = "/images/restaurant1.jpg",
            Address = "123 Đường ABC, Q.1, TP.HCM",
            OpenTime = new TimeSpan(6, 0, 0),
            CloseTime = new TimeSpan(22, 0, 0),
            Lat = 10.762622m,
            Lng = 106.660172m
        };

        private static readonly CategoryEntity PhoCategory = new() { IdCategory = 1, Name = "Phở", Icon = "fas fa-bowl-food" };
        private static readonly CategoryEntity AppetizerCategory = new() { IdCategory = 2, Name = "Món khai vị", Icon = "fas fa-leaf" };

        private static readonly List<Food> SampleMenu = new()
        {
            new Food { IdFood = 1, IdCategory = PhoCategory.IdCategory, IdRestaurant = SampleRestaurant.IdRestaurant, Name = "Phở bò đặc biệt", Description = "Phở bò ngon với nước dùng thơm", Image = "/images/food1.jpg", Video = "", Price = 79000m, Discount = 0m, CookCount = 120, PrepTime = 15, Category = PhoCategory, Restaurant = SampleRestaurant },
            new Food { IdFood = 2, IdCategory = PhoCategory.IdCategory, IdRestaurant = SampleRestaurant.IdRestaurant, Name = "Phở gà", Description = "Phở gà mềm mại với nước dùng thanh", Image = "/images/food2.jpg", Video = "", Price = 68000m, Discount = 0m, CookCount = 90, PrepTime = 12, Category = PhoCategory, Restaurant = SampleRestaurant },
            new Food { IdFood = 3, IdCategory = AppetizerCategory.IdCategory, IdRestaurant = SampleRestaurant.IdRestaurant, Name = "Gỏi cuốn", Description = "Gỏi cuốn tôm thịt tươi ngon", Image = "/images/food3.jpg", Video = "", Price = 55000m, Discount = 0m, CookCount = 75, PrepTime = 10, Category = AppetizerCategory, Restaurant = SampleRestaurant }
        };

        private static readonly List<Order> SampleOrders = new()
        {
            new Order { IdOrder = 2001, IdUser = 1, Total = 185000m, ShippingFee = 15000m, Discount = 0m, FinalTotal = 200000m, Status = "confirmed", CreatedAt = DateTime.Now.AddMinutes(-30), EstimatedDelivery = DateTime.Now.AddMinutes(25), DriverName = "Nguyễn Văn C" },
            new Order { IdOrder = 2002, IdUser = 2, Total = 98000m, ShippingFee = 15000m, Discount = 0m, FinalTotal = 113000m, Status = "confirmed", CreatedAt = DateTime.Now.AddMinutes(-15), EstimatedDelivery = DateTime.Now.AddMinutes(40), DriverName = "Lê Thị D" }
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