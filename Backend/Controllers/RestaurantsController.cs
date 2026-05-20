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
        private readonly RestService _restaurantService;

        public RestaurantsController(ILogger<RestaurantsController> logger, RestService restaurantService)
        {
            _logger = logger;
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<Restaurant>>>> GetAllRestaurants(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] int? categoryId = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 6;
            if (pageSize > 50) pageSize = 50;

            var pagedResult = await _restaurantService.GetPagedRestaurantsAsync(page, pageSize, categoryId);

            return Ok(new ApiResponse<PagedResult<Restaurant>>
            {
                Code = 200,
                Message = "Success",
                Results = pagedResult
            });
        }

        [HttpGet("{id}")]
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
    }
}
