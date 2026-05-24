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

        public async Task<IActionResult> Index(int? categoryId = null)
        {
            var foods = categoryId.HasValue
                ? await _apiClient.GetResultAsync<List<Food>>($"Food/category/{categoryId.Value}", false)
                : await _apiClient.GetResultAsync<List<Food>>("Food", false);

            ViewBag.Categories = await _apiClient.GetResultAsync<List<Category>>("Category", false) ?? new List<Category>();
            ViewBag.SelectedCategoryId = categoryId;

            return View(foods ?? new List<Food>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var food = await _apiClient.GetResultAsync<Food>($"Food/{id}", false);
            return food == null ? NotFound() : View(food);
        }
    }
}
