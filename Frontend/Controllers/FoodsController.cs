using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using _225DAPM32.Services;

namespace _225DAPM32.Controllers
{
    public class FoodsController : Controller
    {
        private readonly ApiClient _apiClient;

        public FoodsController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IActionResult Index(int? categoryId = null)
        {
            return RedirectToAction("Index", "Restaurants", new { area = "", categoryId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var food = await _apiClient.GetResultAsync<Food>($"Food/{id}", false);
            return food == null
                ? NotFound()
                : RedirectToAction("Details", "Restaurants", new { area = "", id = food.IdRestaurant });
        }
    }
}
