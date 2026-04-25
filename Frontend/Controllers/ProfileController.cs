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
            Id_User = 1,
            Username = "nguyenvana",
            Email = "nguyen.vana@email.com",
            Phone = "0123456789",
            Avatar = "/images/avatar.jpg",
            Status = "active",
            Created_At = new DateTime(2024, 1, 15),
            LastOnline = DateTime.Now,
            Update_Bio = "Thích ăn đồ ngon và giao hàng nhanh!",
            Current_Lat = 10.762622m,
            Current_Lng = 106.660172m,
            Cancel_Rate = 0.02f
        };

        // Giả lập đơn hàng
        private static List<Order> userOrders = new List<Order>
        {
            new Order {
                Id_Order = 1,
                Id_User = 1,
                Status = "completed",
                Total = 150000,
                ShippingFee = 25000,
                Created_At = DateTime.Now.AddDays(-2),
                Delivered_At = DateTime.Now.AddDays(-1),
                TrackingNumber = "VN123456789",
                DriverName = "Nguyễn Văn Tài",
                DriverPhone = "0987654321"
            },
            new Order {
                Id_Order = 2,
                Id_User = 1,
                Status = "delivering",
                Total = 85000,
                ShippingFee = 25000,
                Created_At = DateTime.Now.AddHours(-5),
                Delivered_At = null,
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
                Id_Address = 1,
                Id_User = 1,
                Name = "Nguyễn Văn A",
                Phone = "0123456789",
                AddressDetail = "123 Đường ABC, Quận 1, TP.HCM",
                Is_Default = true
            },
            new Address {
                Id_Address = 2,
                Id_User = 1,
                Name = "Nguyễn Văn A",
                Phone = "0123456789",
                AddressDetail = "456 Đường XYZ, Quận 2, TP.HCM",
                Is_Default = false
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
            currentUser.Update_Bio = bio;

            TempData["Success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Settings");
        }

        // Static methods for cross-controller access
        public static int GetNextOrderId()
        {
            return userOrders.Count > 0 ? userOrders.Max(o => o.Id_Order) + 1 : 1;
        }

        public static void AddOrder(Order order)
        {
            userOrders.Add(order);
        }

        public static Order GetOrderById(int orderId)
        {
            return userOrders.FirstOrDefault(o => o.Id_Order == orderId);
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