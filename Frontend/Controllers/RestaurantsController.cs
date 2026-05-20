using Microsoft.AspNetCore.Mvc;
using _225DAPM32.Models;
using System.Text.Json;

namespace _225DAPM32.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public RestaurantsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6, int? categoryId = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var token = HttpContext.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // Build query string
                var query = $"Restaurants?page={page}&pageSize={pageSize}";
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    query += $"&categoryId={categoryId.Value}";
                }

                // Fetch restaurants
                var response = await client.GetAsync(query);

                List<Restaurant> restaurants = new();
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedResult<Restaurant>>>(content, _jsonOptions);

                    if (apiResponse?.Results != null)
                    {
                        var pagedResult = apiResponse.Results;
                        ViewBag.CurrentPage = pagedResult.Page;
                        ViewBag.TotalPages = pagedResult.TotalPages;
                        ViewBag.TotalItems = pagedResult.TotalItems;
                        restaurants = pagedResult.Items;
                    }
                    else
                    {
                        SetEmptyPagination();
                    }
                }
                else
                {
                    SetEmptyPagination();
                }

                // Fetch categories
                var catResponse = await client.GetAsync("Category");
                if (catResponse.IsSuccessStatusCode)
                {
                    var catContent = await catResponse.Content.ReadAsStringAsync();
                    var catApiResponse = JsonSerializer.Deserialize<ApiResponse<List<Category>>>(catContent, _jsonOptions);
                    ViewBag.Categories = catApiResponse?.Results ?? new List<Category>();
                }
                else
                {
                    ViewBag.Categories = new List<Category>();
                }

                ViewBag.SelectedCategoryId = categoryId;
                return View(restaurants);
            }
            catch (Exception)
            {
                SetEmptyPagination();
                ViewBag.Categories = new List<Category>();
                ViewBag.SelectedCategoryId = categoryId;
                return View(new List<Restaurant>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Filter(int page = 1, int pageSize = 6, int? categoryId = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var token = HttpContext.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var query = $"Restaurants?page={page}&pageSize={pageSize}";
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    query += $"&categoryId={categoryId.Value}";
                }

                var response = await client.GetAsync(query);

                List<Restaurant> restaurants = new();
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedResult<Restaurant>>>(content, _jsonOptions);

                    if (apiResponse?.Results != null)
                    {
                        var pagedResult = apiResponse.Results;
                        ViewBag.CurrentPage = pagedResult.Page;
                        ViewBag.TotalPages = pagedResult.TotalPages;
                        ViewBag.TotalItems = pagedResult.TotalItems;
                        restaurants = pagedResult.Items;
                    }
                    else
                    {
                        SetEmptyPagination();
                    }
                }
                else
                {
                    SetEmptyPagination();
                }

                ViewBag.SelectedCategoryId = categoryId;
                return PartialView("_RestaurantGrid", restaurants);
            }
            catch (Exception)
            {
                SetEmptyPagination();
                ViewBag.SelectedCategoryId = categoryId;
                return PartialView("_RestaurantGrid", new List<Restaurant>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("API");

                var token = HttpContext.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // Fetch restaurant details
                var response = await client.GetAsync($"Restaurants/{id}");

                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<Restaurant>>(content, _jsonOptions);

                if (apiResponse?.Results == null)
                    return NotFound();

                // Fetch foods by restaurant
                var foodResponse = await client.GetAsync($"Food/restaurant/{id}");
                if (foodResponse.IsSuccessStatusCode)
                {
                    var foodContent = await foodResponse.Content.ReadAsStringAsync();
                    var foodApiResponse = JsonSerializer.Deserialize<ApiResponse<List<Food>>>(foodContent, _jsonOptions);
                    ViewBag.Foods = foodApiResponse?.Results ?? new List<Food>();
                }
                else
                {
                    ViewBag.Foods = new List<Food>();
                }

                return View(apiResponse.Results);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        private void SetEmptyPagination()
        {
            ViewBag.CurrentPage = 1;
            ViewBag.TotalPages = 0;
            ViewBag.TotalItems = 0;
        }
    }
}
