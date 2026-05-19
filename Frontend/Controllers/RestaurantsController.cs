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
            new Models.Restaurant { IdRestaurant = 1, NameRestaurant = "Nhà Hàng Phở 24h", Description = "Phở ngon mọi lúc", Image = "/images/restaurant1.jpg", Address = "123 Đường ABC, TP.HCM", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Lat = 10.762622m, Lng = 106.660172m },
            new Models.Restaurant { IdRestaurant = 2, NameRestaurant = "Gà Rán Crispy", Description = "Gà rán giòn tan", Image = "/images/restaurant2.jpg", Address = "456 Đường XYZ, TP.HCM", OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(23, 0, 0), Lat = 10.762622m, Lng = 106.660172m }
        };

        public IActionResult Index()
        {
            return View(restaurants);
        }

        public IActionResult Details(int id)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.IdRestaurant == id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }
    }
}