using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponse>>>> GetAllCategories()
        {
            try
            {
                var categoryResponses = await _categoryService.GetAllCategoriesAsync();

                return Ok(new ApiResponse<IEnumerable<CategoryResponse>>
                {
                    Code = 1000,
                    Message = "Lấy danh sách danh mục thành công",
                    Results = categoryResponses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CategoryResponse>>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetCategory(int id)
        {
            try
            {
                var categoryResponse = await _categoryService.GetCategoryByIdAsync(id);

                if (categoryResponse == null)
                {
                    return NotFound(new ApiResponse<CategoryResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy danh mục",
                        Results = default!
                    });
                }

                return Ok(new ApiResponse<CategoryResponse>
                {
                    Code = 1000,
                    Message = "Lấy thông tin danh mục thành công",
                    Results = categoryResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CategoryResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> CreateCategory([FromBody] CategoryRequest categoryRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<CategoryResponse>
                    {
                        Code = 1003,
                        Message = "Dữ liệu không hợp lệ",
                        Results = default!
                    });
                }

                var categoryResponse = await _categoryService.CreateCategoryAsync(categoryRequest);

                return CreatedAtAction(nameof(GetCategory), new { id = categoryResponse.IdCategory }, new ApiResponse<CategoryResponse>
                {
                    Code = 1000,
                    Message = "Tạo danh mục thành công",
                    Results = categoryResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CategoryResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> UpdateCategory(int id, [FromBody] CategoryRequest categoryRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<CategoryResponse>
                    {
                        Code = 1003,
                        Message = "Dữ liệu không hợp lệ",
                        Results = default!
                    });
                }

                var categoryResponse = await _categoryService.UpdateCategoryAsync(id, categoryRequest);

                if (categoryResponse == null)
                {
                    return NotFound(new ApiResponse<CategoryResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy danh mục",
                        Results = default!
                    });
                }

                return Ok(new ApiResponse<CategoryResponse>
                {
                    Code = 1000,
                    Message = "Cập nhật danh mục thành công",
                    Results = categoryResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CategoryResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = default!
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);

                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy danh mục",
                        Results = default!
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Xóa danh mục thành công",
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

        [HttpGet("{id}/foods")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FoodResponse>>>> GetCategoryWithFoods(int id)
        {
            try
            {
                var foodResponses = await _categoryService.GetFoodsByCategoryAsync(id);

                if (foodResponses == null)
                {
                    return NotFound(new ApiResponse<IEnumerable<FoodResponse>>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy danh mục",
                        Results = default!
                    });
                }

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
