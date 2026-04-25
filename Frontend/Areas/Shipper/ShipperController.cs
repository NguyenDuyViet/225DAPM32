using Microsoft.AspNetCore.Mvc;
using System.Linq;
using _225DAPM32.Areas.Shipper.Models;
using _225DAPM32.Models;

namespace _225DAPM32.Areas.Shipper
{
    [Area("Shipper")]
    public class ShipperController : Controller
    {
        private static readonly List<Order> CurrentOrders = new()
        {
            new Order { Id_Order = 3001, Id_User = 1, Total = 185000m, ShippingFee = 15000m, FinalTotal = 200000m, Status = "Delivering", Address = null, DriverName = "Hoàng Văn E", DriverPhone = "0911222333", EstimatedDelivery = DateTime.Now.AddMinutes(30) },
            new Order { Id_Order = 3002, Id_User = 2, Total = 98000m, ShippingFee = 15000m, FinalTotal = 113000m, Status = "Delivering", DriverName = "Hoàng Văn E", DriverPhone = "0911222333", EstimatedDelivery = DateTime.Now.AddMinutes(45) }
        };

        private static readonly List<Order> HistoryOrders = new()
        {
            new Order { Id_Order = 3000, Id_User = 3, Total = 130000m, ShippingFee = 15000m, FinalTotal = 145000m, Status = "Completed", Created_At = DateTime.Now.AddHours(-6), Delivered_At = DateTime.Now.AddHours(-5) },
            new Order { Id_Order = 2999, Id_User = 4, Total = 210000m, ShippingFee = 15000m, FinalTotal = 225000m, Status = "Completed", Created_At = DateTime.Now.AddHours(-8), Delivered_At = DateTime.Now.AddHours(-7) }
        };

        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard Shipper";
            var model = new ShipperDashboardViewModel
            {
                ActiveDeliveries = CurrentOrders.Count,
                CompletedDeliveries = HistoryOrders.Count,
                TodayEarnings = 450000m,
                Rating = 4.8m
            };
            return View(model);
        }

        public IActionResult Orders()
        {
            ViewData["Title"] = "Đơn hàng cần giao";
            return View(CurrentOrders);
        }

        public IActionResult History()
        {
            ViewData["Title"] = "Lịch sử giao hàng";
            return View(HistoryOrders);
        }

        public IActionResult Earnings()
        {
            ViewData["Title"] = "Thu nhập";
            var earnings = new ShipperEarningsViewModel
            {
                WeekTotal = 2850000m,
                TodayTotal = 450000m,
                CompletedOrders = HistoryOrders.Count,
                PendingOrders = CurrentOrders.Count
            };
            return View(earnings);
        }

        public IActionResult Profile()
        {
            ViewData["Title"] = "Thông tin cá nhân";
            var profile = new ShipperProfileViewModel
            {
                Name = "Hoàng Văn E",
                Phone = "0911222333",
                Email = "shipper@example.com",
                Status = "Online",
                Rating = 4.8m,
                CompletedDeliveries = HistoryOrders.Count
            };
            return View(profile);
        }

        public IActionResult Settings()
        {
            ViewData["Title"] = "Cài đặt";
            return View();
        }
    }
}