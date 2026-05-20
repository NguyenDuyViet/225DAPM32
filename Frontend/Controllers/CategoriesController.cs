using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using System.Text.Json;

namespace _225DAPM32.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public CategoriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var response = await client.GetAsync("Category");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Category>>>(content, _jsonOptions);

                    if (apiResponse?.Results != null)
                    {
                        return View(apiResponse.Results);
                    }
                }

                return View(new List<Category>());
            }
            catch (Exception)
            {
                return View(new List<Category>());
            }
        }

        public async Task<IActionResult> Foods(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                // Lấy thông tin category
                var catResponse = await client.GetAsync($"Category/{id}");
                if (catResponse.IsSuccessStatusCode)
                {
                    var catContent = await catResponse.Content.ReadAsStringAsync();
                    var catApiResponse = JsonSerializer.Deserialize<ApiResponse<Category>>(catContent, _jsonOptions);
                    ViewBag.Category = catApiResponse?.Results;
                }

                // Lấy danh sách food theo category
                var foodResponse = await client.GetAsync($"Food/category/{id}");

                if (foodResponse.IsSuccessStatusCode)
                {
                    var foodContent = await foodResponse.Content.ReadAsStringAsync();
                    var foodApiResponse = JsonSerializer.Deserialize<ApiResponse<List<Food>>>(foodContent, _jsonOptions);

                    if (foodApiResponse?.Results != null)
                    {
                        return View(foodApiResponse.Results);
                    }
                }

                return View(new List<Food>());
            }
            catch (Exception)
            {
                return View(new List<Food>());
            }
        }
    }
}
