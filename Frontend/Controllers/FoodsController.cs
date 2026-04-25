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
            new Food { Id_Food = 1, Id_Category = 1, Id_Restaurant = 1, Name = "Phở Bò", Description = "Món phở truyền thống", Image = "/images/pho.jpg", Price = 30000, Discount = 0, Cook_Count = 100, Prep_Time = 15 },
            new Food { Id_Food = 2, Id_Category = 1, Id_Restaurant = 1, Name = "Bún Bò Huế", Description = "Bún bò cay ngon", Image = "/images/bunbo.jpg", Price = 35000, Discount = 5000, Cook_Count = 80, Prep_Time = 20 },
            new Food { Id_Food = 3, Id_Category = 2, Id_Restaurant = 2, Name = "Gà Rán", Description = "Gà rán giòn tan", Image = "/images/garan.jpg", Price = 50000, Discount = 0, Cook_Count = 150, Prep_Time = 10 }
        };

        public IActionResult Index()
        {
            return View(foods);
        }

        public IActionResult Details(int id)
        {
            var food = foods.FirstOrDefault(f => f.Id_Food == id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }
    }
}