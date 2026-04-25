using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using System.Collections.Generic;

namespace _225DAPM32.Controllers
{
    public class CartController : Controller
    {
        // Giả lập giỏ hàng
        private static List<Cart_Food> cartItems = new List<Cart_Food>
        {
            new Cart_Food { Id_CartFood = 1, Id_Cart = 1, Id_Food = 1, Quantity = 2, Note = "Ít cay" },
            new Cart_Food { Id_CartFood = 2, Id_Cart = 1, Id_Food = 3, Quantity = 1, Note = "" }
        };

        public IActionResult Index()
        {
            // Giả lập join với Food
            var cartWithFoods = cartItems.Select(cf => new
            {
                CartFood = cf,
                Food = new Food { Id_Food = cf.Id_Food, Name = cf.Id_Food == 1 ? "Phở Bò" : "Gà Rán", Price = cf.Id_Food == 1 ? 30000m : 50000m, Image = cf.Id_Food == 1 ? "/images/pho.jpg" : "/images/garan.jpg" }
            }).ToList();

            return View(cartWithFoods);
        }

        [HttpPost]
        public IActionResult AddToCart(int foodId, int quantity, string note)
        {
            // Thêm vào giỏ hàng
            var newItem = new Cart_Food
            {
                Id_CartFood = cartItems.Count + 1,
                Id_Cart = 1, // Giả lập user 1
                Id_Food = foodId,
                Quantity = quantity,
                Note = note
            };
            cartItems.Add(newItem);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            if (!cartItems.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            // Tạo đơn hàng mới
            var newOrder = new Order
            {
                Id_Order = ProfileController.GetNextOrderId(),
                Id_User = 1, // Giả lập user 1
                Status = "pending",
                Total = cartItems.Sum(cf => (cf.Id_Food == 1 ? 30000m : 50000m) * cf.Quantity), // Giả lập giá
                ShippingFee = 25000,
                Created_At = DateTime.Now,
                Delivered_At = null
            };

            // Thêm đơn hàng vào danh sách của ProfileController
            ProfileController.AddOrder(newOrder);

            // Xóa giỏ hàng sau khi đặt hàng
            cartItems.Clear();

            TempData["Success"] = $"Đặt hàng thành công! Mã đơn hàng: #{newOrder.Id_Order}";
            return RedirectToAction("OrderSuccess", new { orderId = newOrder.Id_Order });
        }

        public IActionResult OrderSuccess(int orderId)
        {
            var order = ProfileController.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}