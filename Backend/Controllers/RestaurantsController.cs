using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly ILogger<RestaurantsController> _logger;
        private RestService _restaurantService;

        public RestaurantsController(ILogger<RestaurantsController> logger, RestService restaurantService)
        {
            _logger = logger;
            _restaurantService = restaurantService;
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<Restaurant>>> GetAllRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            if (restaurants == null || !restaurants.Any())
                return NotFound(new { message = "Internal server error" });
            return Ok(new ApiResponse<List<Restaurant>>
            {
                Code = 200,
                Message = "Success",
                Results = restaurants
            });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<Restaurant>>> GetRestaurantById(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return NotFound(new { message = "Restaurant not found" });
            return Ok(new ApiResponse<Restaurant>
            {
                Code = 200,
                Message = "Success",
                Results = restaurant
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteRestaurant(int id)
        {
            var result = await _restaurantService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Restaurant not found" });
            return Ok(new ApiResponse<bool>
            {
                Code = 200,
                Message = "Success",
                Results = result
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,restaurants")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateRestaurant(int id, RestRequestDTO rest)
        {
            var result = await _restaurantService.UpdateRestAsync(id, rest);
            if (!result)
                return NotFound(new { message = "Restaurant not found" });
            return Ok(new ApiResponse<bool>
            {
                Code = 200,
                Message = "Success",
                Results = result
            });
        }

        [NonAction]
        public async Task<ActionResult<ApiResponse<string>>> ConfirmOrder(int idOrder)
        {
            return null;
        }

        [NonAction]
        public async Task<ActionResult<ApiResponse<string>>> CancelOrder(int idOrder)
        {
            return null;
        }
    }
}

