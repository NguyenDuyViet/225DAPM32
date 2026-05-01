using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using System.Collections.Generic;

namespace _225DAPM32.Controllers
{
    public class ProfileController : Controller
    {
        // Giả lập dữ liệu user
        private static User currentUser = new User
        {
            IdUser = 1,
            Username = "nguyenvana",
            Email = "nguyen.vana@email.com",
            Phone = "0123456789",
            Avatar = "/images/avatar.jpg",
            Status = "active",
            CreatedAt = new DateTime(2024, 1, 15),
            LastOnline = DateTime.Now,
            UpdateBio = "Thích ăn đồ ngon và giao hàng nhanh!",
            CurrentLat = 10.762622m,
            CurrentLng = 106.660172m,
            CancelRate = 0.02f,
            IdRole = 1
        };

        // Giả lập đơn hàng
        private static List<Order> userOrders = new List<Order>
        {
            new Order {
                IdOrder = 1,
                IdUser = 1,
                Status = "completed",
                Total = 150000,
                ShippingFee = 25000,
                CreatedAt = DateTime.Now.AddDays(-2),
                DeliveredAt = DateTime.Now.AddDays(-1),
                TrackingNumber = "VN123456789",
                DriverName = "Nguyễn Văn Tài",
                DriverPhone = "0987654321"
            },
            new Order {
                IdOrder = 2,
                IdUser = 1,
                Status = "delivering",
                Total = 85000,
                ShippingFee = 25000,
                CreatedAt = DateTime.Now.AddHours(-5),
                DeliveredAt = null,
                TrackingNumber = "VN987654321",
                DriverName = "Trần Thị Linh",
                DriverPhone = "0123456789",
                EstimatedDelivery = DateTime.Now.AddMinutes(30)
            }
        };

        // Giả lập địa chỉ
        private static List<Address> userAddresses = new List<Address>
        {
            new Address {
                IdAddress = 1,
                IdUser = 1,
                Name = "Nguyễn Văn A",
                Phone = "0123456789",
                AddressDetail = "123 Đường ABC, Quận 1, TP.HCM",
                IsDefault = true
            },
            new Address {
                IdAddress = 2,
                IdUser = 1,
                Name = "Nguyễn Văn A",
                Phone = "0123456789",
                AddressDetail = "456 Đường XYZ, Quận 2, TP.HCM",
                IsDefault = false
            }
        };

        public IActionResult Index()
        {
            ViewBag.User = currentUser;
            ViewBag.Orders = userOrders;
            ViewBag.Addresses = userAddresses;
            return View();
        }

        public IActionResult Orders()
        {
            ViewBag.User = currentUser;
            ViewBag.Orders = userOrders;
            return View();
        }

        public IActionResult Addresses()
        {
            ViewBag.User = currentUser;
            ViewBag.Addresses = userAddresses;
            return View();
        }

        public IActionResult Settings()
        {
            ViewBag.User = currentUser;
            return View();
        }

        [HttpPost]
        public IActionResult UpdateProfile(string username, string email, string phone, string bio)
        {
            // Cập nhật thông tin user (giả lập)
            currentUser.Username = username;
            currentUser.Email = email;
            currentUser.Phone = phone;
            currentUser.UpdateBio = bio;

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Settings");
        }

        // Static methods for cross-controller access
        public static int GetNextOrderId()
        {
            return userOrders.Count > 0 ? userOrders.Max(o => o.IdOrder) + 1 : 1;
        }

        public static void AddOrder(Order order)
        {
            userOrders.Add(order);
        }

        public static Order GetOrderById(int orderId)
        {
            return userOrders.FirstOrDefault(o => o.IdOrder == orderId);
        }

        public IActionResult OrderTracking(int id)
        {
            var order = GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.User = currentUser;
            return View(order);
        }
    }
}