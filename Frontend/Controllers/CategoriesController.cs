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

        public IActionResult Foods(int id)
        {
            return RedirectToAction("Index", "Restaurants", new { area = "", categoryId = id });
        }
    }
}
