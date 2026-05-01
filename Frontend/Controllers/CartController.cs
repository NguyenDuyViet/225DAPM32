using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using System.Collections.Generic;

namespace _225DAPM32.Controllers
{
    public class CartController : Controller
    {
        // Giả lập giỏ hàng
        private static List<CartFood> cartItems = new List<CartFood>
        {
            new CartFood { IdCartFood = 1, IdCart = 1, IdFood = 1, Quantity = 2, Note = "Ít cay" },
            new CartFood { IdCartFood = 2, IdCart = 1, IdFood = 3, Quantity = 1, Note = "" }
        };

        public IActionResult Index()
        {
            // Giả lập join với Food
            var cartWithFoods = cartItems.Select(cf => new
            {
                CartFood = cf,
                Food = new Food { IdFood = cf.IdFood, Name = cf.IdFood == 1 ? "Phở Bò" : "Gà Rán", Price = cf.IdFood == 1 ? 30000m : 50000m, Image = cf.IdFood == 1 ? "/images/pho.jpg" : "/images/garan.jpg" }
            }).ToList();

            return View(cartWithFoods);
        }

        [HttpPost]
        public IActionResult AddToCart(int foodId, int quantity, string note)
        {
            // Thêm vào giỏ hàng
            var newItem = new CartFood
            {
                IdCartFood = cartItems.Count + 1,
                IdCart = 1, // Giả lập user 1
                IdFood = foodId,
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
                IdOrder = ProfileController.GetNextOrderId(),
                IdUser = 1, // Giả lập user 1
                Status = "pending",
                Total = cartItems.Sum(cf => (cf.IdFood == 1 ? 30000m : 50000m) * cf.Quantity), // Giả lập giá
                ShippingFee = 25000,
                CreatedAt = DateTime.Now,
                DeliveredAt = null
            };

            // Thêm đơn hàng vào danh sách của ProfileController
            ProfileController.AddOrder(newOrder);

            // Xóa giỏ hàng sau khi đặt hàng
            cartItems.Clear();

            TempData["Success"] = $"Đặt hàng thành công! Mã đơn hàng: #{newOrder.IdOrder}";
            return RedirectToAction("OrderSuccess", new { orderId = newOrder.IdOrder });
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