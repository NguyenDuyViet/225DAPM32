using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using System.Collections.Generic;

namespace _225DAPM32.Controllers
{
    public class RestaurantsController : Controller
    {
        // Giả lập dữ liệu
        private static List<Models.Restaurant> restaurants = new List<Models.Restaurant>
        {
            new Models.Restaurant { Id_Restaurant = 1, Name_Restaurant = "Nhà Hàng Phở 24h", Description = "Phở ngon mọi lúc", Image = "/images/restaurant1.jpg", Address = "123 Đường ABC, TP.HCM", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
            new Models.Restaurant { Id_Restaurant = 2, Name_Restaurant = "Gà Rán Crispy", Description = "Gà rán giòn tan", Image = "/images/restaurant2.jpg", Address = "456 Đường XYZ, TP.HCM", OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(23, 0, 0), Lat = 10.762622m, Lng = 106.660172m }
        };

        public IActionResult Index()
        {
            return View(restaurants);
        }

        public IActionResult Details(int id)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Id_Restaurant == id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        public IActionResult Menu(int id)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Id_Restaurant == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            // Giả lập menu cho nhà hàng
            var menuItems = GetMenuItemsForRestaurant(id);
            ViewBag.Restaurant = restaurant;
            return View(menuItems);
        }

        private List<Food> GetMenuItemsForRestaurant(int restaurantId)
        {
            // Giả lập dữ liệu menu cho từng nhà hàng
            var allFoods = new List<Food>
            {
                // Menu cho nhà hàng Phở 24h
                new Food { Id_Food = 1, Name = "Phở Bò", Description = "Phở bò tái, nạm, gân, sách", Price = 45000, Image = "/images/pho-bo.jpg", Id_Restaurant = 1, Id_Category = 1 },
                new Food { Id_Food = 2, Name = "Phở Gà", Description = "Phở gà ta thả vườn", Price = 40000, Image = "/images/pho-ga.jpg", Id_Restaurant = 1, Id_Category = 1 },
                new Food { Id_Food = 3, Name = "Phở Tái", Description = "Phở tái nạm", Price = 35000, Image = "/images/pho-tai.jpg", Id_Restaurant = 1, Id_Category = 1 },

                // Menu cho nhà hàng Gà Rán Crispy
                new Food { Id_Food = 4, Name = "Gà Rán 1 Miếng", Description = "1 miếng gà rán giòn tan", Price = 35000, Image = "/images/ga-ran-1.jpg", Id_Restaurant = 2, Id_Category = 2 },
                new Food { Id_Food = 5, Name = "Gà Rán 2 Miếng", Description = "2 miếng gà rán combo", Price = 65000, Image = "/images/ga-ran-2.jpg", Id_Restaurant = 2, Id_Category = 2 },
                new Food { Id_Food = 6, Name = "Combo Gà Rán", Description = "3 miếng gà + khoai tây + nước", Price = 95000, Image = "/images/combo-ga.jpg", Id_Restaurant = 2, Id_Category = 2 },
                new Food { Id_Food = 7, Name = "Khoai Tây Chiên", Description = "Khoai tây chiên giòn", Price = 25000, Image = "/images/khoai-tay.jpg", Id_Restaurant = 2, Id_Category = 2 }
            };

            return allFoods.Where(f => f.Id_Restaurant == restaurantId).ToList();
        }
    }
}