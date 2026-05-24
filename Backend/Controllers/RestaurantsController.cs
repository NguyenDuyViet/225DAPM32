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
        private readonly OrderService _orderService;

        public RestaurantsController(ILogger<RestaurantsController> logger, RestService restaurantService, OrderService orderService)
        {
            _logger = logger;
            _restaurantService = restaurantService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<Restaurant>>>> GetAllRestaurants(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? district = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 6;
            if (pageSize > 50) pageSize = 50;

            var pagedResult = await _restaurantService.GetPagedRestaurantsAsync(page, pageSize, categoryId, search, district, minPrice, maxPrice);

            return Ok(new ApiResponse<PagedResult<Restaurant>>
            {
                Code = 1000,
                Message = "Lấy danh sách nhà hàng thành công",
                Results = pagedResult
            });
        }

        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<List<Restaurant>>>> GetAllRestaurantsFlat()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();

            return Ok(new ApiResponse<List<Restaurant>>
            {
                Code = 1000,
                Message = "Lấy danh sách nhà hàng thành công",
                Results = restaurants
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
                Code = 1000,
                Message = "Lấy thông tin nhà hàng thành công",
                Results = restaurant
            });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<Restaurant>>> CreateRestaurant([FromBody] RestRequestDTO rest)
        {
            var restaurant = await _restaurantService.CreateRestaurantAsync(rest);

            return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.IdRestaurant }, new ApiResponse<Restaurant>
            {
                Code = 1000,
                Message = "Tạo nhà hàng thành công",
                Results = restaurant
            });
        }

        [HttpGet("{id}/dashboard")]
        public async Task<ActionResult<ApiResponse<object>>> GetDashboardStats(int id)
        {
            var stats = await _orderService.GetDashboardStatsAsync(id);
            return Ok(new ApiResponse<object>
            {
                Code = 200,
                Message = "Success",
                Results = stats
            });
        }

        [HttpGet("{id}/analytics")]
        public async Task<ActionResult<ApiResponse<object>>> GetAnalyticsStats(int id)
        {
            var stats = await _orderService.GetAnalyticsStatsAsync(id);
            return Ok(new ApiResponse<object>
            {
                Code = 200,
                Message = "Success",
                Results = stats
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
                Code = 1000,
                Message = "Xóa nhà hàng thành công",
                Results = result
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,restaurant")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateRestaurant(int id, RestRequestDTO rest)
        {
            var result = await _restaurantService.UpdateRestAsync(id, rest);
            if (!result)
                return NotFound(new { message = "Restaurant not found" });

            return Ok(new ApiResponse<bool>
            {
                Code = 1000,
                Message = "Cập nhật nhà hàng thành công",
                Results = result
            });
        }

        // GET: api/Restaurants/{id}/reviews
        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetRestaurantReviews(int id)
        {
            var reviews = await _orderService.GetRestaurantReviewsAsync(id);
            return Ok(new ApiResponse<IEnumerable<object>>
            {
                Code = 200,
                Message = "Success",
                Results = reviews
            });
        }
    }
}
