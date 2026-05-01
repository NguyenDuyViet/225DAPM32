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
            new Order { IdOrder = 3001, IdUser = 1, Total = 185000m, ShippingFee = 15000m, FinalTotal = 200000m, Status = "delivering", DriverName = "Hoàng Văn E", DriverPhone = "0911222333", EstimatedDelivery = DateTime.Now.AddMinutes(30) },
            new Order { IdOrder = 3002, IdUser = 2, Total = 98000m, ShippingFee = 15000m, FinalTotal = 113000m, Status = "delivering", DriverName = "Hoàng Văn E", DriverPhone = "0911222333", EstimatedDelivery = DateTime.Now.AddMinutes(45) }
        };

        private static readonly List<Order> HistoryOrders = new()
        {
            new Order { IdOrder = 3000, IdUser = 3, Total = 130000m, ShippingFee = 15000m, FinalTotal = 145000m, Status = "completed", CreatedAt = DateTime.Now.AddHours(-6), DeliveredAt = DateTime.Now.AddHours(-5) },
            new Order { IdOrder = 2999, IdUser = 4, Total = 210000m, ShippingFee = 15000m, FinalTotal = 225000m, Status = "completed", CreatedAt = DateTime.Now.AddHours(-8), DeliveredAt = DateTime.Now.AddHours(-7) }
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