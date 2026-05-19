using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly FoodRepository _foodRepository;
        private readonly IMapper _mapper;

        public FoodController(FoodRepository foodRepository, IMapper mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        // GET: api/Food
        [HttpGet]

        public async Task<ActionResult<ApiResponse<IEnumerable<FoodResponse>>>> GetAllFoods()
        {
            try
            {
                var foods = await _foodRepository.GetAllAsync();

                var foodResponses = _mapper.Map<IEnumerable<FoodResponse>>(foods);
                
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
                    Results = null
                });
            }
        }

        // GET: api/Food/5
        [HttpGet("{id}")]

        public async Task<ActionResult<ApiResponse<FoodResponse>>> GetFood(int id)
        {
            try
            {
                var food = await _foodRepository.GetByIdAsync(id);

                if (food == null)
                {

                    return NotFound(new ApiResponse<FoodResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy món ăn",
                        Results = null
                    });
                }


                var foodResponse = _mapper.Map<FoodResponse>(food);
                
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
                    Results = null
                });
            }
        }

        // POST: api/Food
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
                        Results = null
                    });
                }

                var food = _mapper.Map<Food>(foodRequest);
                await _foodRepository.AddAsync(food);
                await _foodRepository.SaveChangesAsync();

                var foodResponse = _mapper.Map<FoodResponse>(food);
                
                return CreatedAtAction(nameof(GetFood), new { id = food.IdFood }, new ApiResponse<FoodResponse>
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
                    Results = null
                });
            }
        }

        // PUT: api/Food/5
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
                        Results = null
                    });
                }

                var existingFood = await _foodRepository.GetByIdAsync(id);
                if (existingFood == null)
                {

                    return NotFound(new ApiResponse<FoodResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy món ăn",
                        Results = null
                    });
                }

                _mapper.Map(foodRequest, existingFood);
                _foodRepository.Update(existingFood);
                await _foodRepository.SaveChangesAsync();


                var foodResponse = _mapper.Map<FoodResponse>(existingFood);
                
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
                    Results = null
                });
            }
        }

        // DELETE: api/Food/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteFood(int id)
        {
            try
            {
                var food = await _foodRepository.GetByIdAsync(id);
                if (food == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy món ăn",
                        Results = null
                    });
                }

                await _foodRepository.DeleteAsync(food);
                await _foodRepository.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Xóa món ăn thành công",
                    Results = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = null
                });
            }
        }

        // GET: api/Food/restaurant/5
        [HttpGet("restaurant/{restaurantId}")]

        public async Task<ActionResult<ApiResponse<IEnumerable<FoodResponse>>>> GetFoodsByRestaurant(int restaurantId)
        {
            try
            {
                var foods = await _foodRepository.FindAsync(f => f.IdRestaurant == restaurantId);

                var foodResponses = _mapper.Map<IEnumerable<FoodResponse>>(foods);
                
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
                    Results = null
                });
            }
        }

        // GET: api/Food/category/5
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FoodResponse>>>> GetFoodsByCategory(int categoryId)
        {
            try
            {
                var foods = await _foodRepository.FindAsync(f => f.IdCategory == categoryId);
                var foodResponses = _mapper.Map<IEnumerable<FoodResponse>>(foods);
                
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
                    Results = null
                });
            }
        }
    }
}
