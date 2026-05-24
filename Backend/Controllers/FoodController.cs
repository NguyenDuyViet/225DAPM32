using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly FoodService _foodService;

        public FoodController(FoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<FoodResponse>>>> GetAllFoods()
        {
            try
            {
                var foodResponses = await _foodService.GetAllFoodsAsync();

                return Ok(new ApiResponse<IEnumerable<FoodResponse>>
                {
                    Code = 1000,
                    Message = "Lấy danh sách món ăn thành công",
                    Results = foodResponses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<FoodResponse>>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<FoodResponse>>> GetFood(int id)
        {
            try
            {
                var foodResponse = await _foodService.GetFoodByIdAsync(id);

                if (foodResponse == null)
                {
                    return NotFound(new ApiResponse<FoodResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy món ăn",
                        Results = default!
                    });
                }

                return Ok(new ApiResponse<FoodResponse>
                {
                    Code = 1000,
                    Message = "Lấy thông tin món ăn thành công",
                    Results = foodResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<FoodResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<FoodResponse>>> CreateFood([FromBody] FoodRequest foodRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<FoodResponse>
                    {
                        Code = 1003,
                        Message = "Dữ liệu không hợp lệ",
                        Results = default!
                    });
                }

                var foodResponse = await _foodService.CreateFoodAsync(foodRequest);

                return CreatedAtAction(nameof(GetFood), new { id = foodResponse.IdFood }, new ApiResponse<FoodResponse>
                {
                    Code = 1000,
                    Message = "Tạo món ăn thành công",
                    Results = foodResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<FoodResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<FoodResponse>>> UpdateFood(int id, [FromBody] FoodRequest foodRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<FoodResponse>
                    {
                        Code = 1003,
                        Message = "Dữ liệu không hợp lệ",
                        Results = default!
                    });
                }

                var foodResponse = await _foodService.UpdateFoodAsync(id, foodRequest);

                if (foodResponse == null)
                {
                    return NotFound(new ApiResponse<FoodResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy món ăn",
                        Results = default!
                    });
                }

                return Ok(new ApiResponse<FoodResponse>
                {
                    Code = 1000,
                    Message = "Cập nhật món ăn thành công",
                    Results = foodResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<FoodResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteFood(int id)
        {
            try
            {
                var result = await _foodService.DeleteFoodAsync(id);

                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy món ăn",
                        Results = default!
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Xóa món ăn thành công",
                    Results = default!
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FoodResponse>>>> GetFoodsByRestaurant(int restaurantId)
        {
            try
            {
                var foodResponses = await _foodService.GetFoodsByRestaurantAsync(restaurantId);

                return Ok(new ApiResponse<IEnumerable<FoodResponse>>
                {
                    Code = 1000,
                    Message = "Lấy danh sách món ăn theo nhà hàng thành công",
                    Results = foodResponses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<FoodResponse>>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FoodResponse>>>> GetFoodsByCategory(int categoryId)
        {
            try
            {
                var foodResponses = await _foodService.GetFoodsByCategoryAsync(categoryId);

                return Ok(new ApiResponse<IEnumerable<FoodResponse>>
                {
                    Code = 1000,
                    Message = "Lấy danh sách món ăn theo danh mục thành công",
                    Results = foodResponses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<FoodResponse>>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }
    }
}
