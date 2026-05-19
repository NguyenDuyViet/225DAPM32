using AutoMapper;
using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(CategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponse>>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var categoryResponses = _mapper.Map<IEnumerable<CategoryResponse>>(categories);
                
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
                    Results = null
                });
            }
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);

                if (category == null)
                {
                    return NotFound(new ApiResponse<CategoryResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy danh mục",
                        Results = null
                    });
                }

                var categoryResponse = _mapper.Map<CategoryResponse>(category);
                
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
                    Results = null
                });
            }
        }

        // POST: api/Category
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
                        Results = null
                    });
                }

                var category = _mapper.Map<Category>(categoryRequest);
                await _categoryRepository.AddAsync(category);
                await _categoryRepository.SaveChangesAsync();

                var categoryResponse = _mapper.Map<CategoryResponse>(category);
                
                return CreatedAtAction(nameof(GetCategory), new { id = category.IdCategory }, new ApiResponse<CategoryResponse>
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
                    Results = null
                });
            }
        }

        // PUT: api/Category/5
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
                        Results = null
                    });
                }

                var existingCategory = await _categoryRepository.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(new ApiResponse<CategoryResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy danh mục",
                        Results = null
                    });
                }

                _mapper.Map(categoryRequest, existingCategory);
                _categoryRepository.Update(existingCategory);
                await _categoryRepository.SaveChangesAsync();

                var categoryResponse = _mapper.Map<CategoryResponse>(existingCategory);
                
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
                    Results = null
                });
            }
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy danh mục",
                        Results = null
                    });
                }

                await _categoryRepository.DeleteAsync(category);
                await _categoryRepository.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Xóa danh mục thành công",
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

        // GET: api/Category/5/foods
        [HttpGet("{id}/foods")]
        public async Task<ActionResult<ApiResponse<IEnumerable<FoodResponse>>>> GetCategoryWithFoods(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse<IEnumerable<FoodResponse>>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy danh mục",
                        Results = null
                    });
                }

                var foodResponses = _mapper.Map<IEnumerable<FoodResponse>>(category.Foods);
                
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
