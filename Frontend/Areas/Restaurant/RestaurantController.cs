using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Areas.Restaurant.Models;
using _225DAPM32.Models;
using _225DAPM32.Services;
using RestaurantEntity = _225DAPM32.Models.Restaurant;

namespace _225DAPM32.Areas.Restaurant
{
    [Area("Restaurant")]
    public class RestaurantController : Controller
    {
        private readonly ApiClient _apiClient;

        public RestaurantController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            if (!RequireRestaurant())
                return RedirectToAction("Index", "Home", new { area = "" });

            var restaurant = await GetManagedRestaurantAsync();
            var foods = restaurant == null
                ? new List<Food>()
                : await _apiClient.GetResultAsync<List<Food>>($"Food/restaurant/{restaurant.IdRestaurant}", false) ?? new List<Food>();
            var orders = restaurant == null
                ? new List<Order>()
                : await _apiClient.GetResultAsync<List<Order>>($"Orders/restaurant/{restaurant.IdRestaurant}") ?? new List<Order>();

            ViewData["Title"] = "Dashboard Nhà hàng";
            return View(new RestaurantDashboardViewModel
            {
                ActiveOrders = orders.Count(o => o.Status != "completed" && o.Status != "canceled"),
                RevenueToday = orders.Where(o => o.CreatedAt.Date == DateTime.Today).Sum(o => o.FinalTotal),
                MenuItems = foods.Count,
                Rating = 4.7m
            });
        }

        public async Task<IActionResult> Orders()
        {
            if (!RequireRestaurant())
                return RedirectToAction("Index", "Home", new { area = "" });

            var restaurant = await GetManagedRestaurantAsync();
            ViewData["Title"] = "Đơn hàng";

            if (restaurant == null)
                return View(new List<Order>());

            return View(await _apiClient.GetResultAsync<List<Order>>($"Orders/restaurant/{restaurant.IdRestaurant}") ?? new List<Order>());
        }

        public IActionResult Analytics()
        {
            ViewData["Title"] = "Thống kê";
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            if (!RequireRestaurant())
                return RedirectToAction("Index", "Home", new { area = "" });

            ViewData["Title"] = "Thông tin Nhà hàng";
            return View(await GetManagedRestaurantAsync() ?? new RestaurantEntity());
        }

        public IActionResult Settings()
        {
            ViewData["Title"] = "Cài đặt";
            return View();
        }

        private async Task<RestaurantEntity?> GetManagedRestaurantAsync()
        {
            var restaurants = await _apiClient.GetResultAsync<List<RestaurantEntity>>("Restaurants/all", false) ?? new List<RestaurantEntity>();
            return restaurants.FirstOrDefault();
        }

        private bool RequireRestaurant()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "restaurant" || role == "admin";
        }
    }
}
