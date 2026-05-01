using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using System.Collections.Generic;

namespace _225DAPM32.Controllers
{
    public class FoodsController : Controller
    {
        // Giả lập dữ liệu vì chưa có DB context
        private static List<Food> foods = new List<Food>
        {
            new Food { IdFood = 1, IdCategory = 1, IdRestaurant = 1, Name = "Phở Bò", Description = "Món phở truyền thống", Image = "/images/pho.jpg", Price = 30000, Discount = 0, CookCount = 100, PrepTime = 15 },
            new Food { IdFood = 2, IdCategory = 1, IdRestaurant = 1, Name = "Bún Bò Huế", Description = "Bún bò cay ngon", Image = "/images/bunbo.jpg", Price = 35000, Discount = 5000, CookCount = 80, PrepTime = 20 },
            new Food { IdFood = 3, IdCategory = 2, IdRestaurant = 2, Name = "Gà Rán", Description = "Gà rán giòn tan", Image = "/images/garan.jpg", Price = 50000, Discount = 0, CookCount = 150, PrepTime = 10 }
        };

        public IActionResult Index()
        {
            return View(foods);
        }

        public IActionResult Details(int id)
        {
            var food = foods.FirstOrDefault(f => f.IdFood == id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }
    }
}