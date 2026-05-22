using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly PromotionService _promotionService;

        public PromotionController(PromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        // GET: api/Promotion/restaurant/5
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PromotionResponse>>>> GetPromotionsByRestaurant(int restaurantId)
        {
            try
            {
                var response = await _promotionService.GetPromotionsByRestaurantAsync(restaurantId);
                return Ok(new ApiResponse<IEnumerable<PromotionResponse>>
                {
                    Code = 1000,
                    Message = "Lấy danh sách khuyến mãi thành công",
                    Results = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<PromotionResponse>>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = null
                });
            }
        }

        // GET: api/Promotion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PromotionResponse>>> GetPromotion(int id)
        {
            try
            {
                var response = await _promotionService.GetPromotionByIdAsync(id);
                if (response == null)
                {
                    return NotFound(new ApiResponse<PromotionResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy chương trình khuyến mãi",
                        Results = null
                    });
                }

                return Ok(new ApiResponse<PromotionResponse>
                {
                    Code = 1000,
                    Message = "Lấy thông tin khuyến mãi thành công",
                    Results = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PromotionResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = null
                });
            }
        }

        // POST: api/Promotion
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PromotionResponse>>> CreatePromotion([FromBody] PromotionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<PromotionResponse>
                    {
                        Code = 1003,
                        Message = "Dữ liệu không hợp lệ",
                        Results = null
                    });
                }

                var response = await _promotionService.CreatePromotionAsync(request);
                return CreatedAtAction(nameof(GetPromotion), new { id = response.IdPromo }, new ApiResponse<PromotionResponse>
                {
                    Code = 1000,
                    Message = "Tạo khuyến mãi thành công",
                    Results = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PromotionResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = null
                });
            }
        }

        // PUT: api/Promotion/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PromotionResponse>>> UpdatePromotion(int id, [FromBody] PromotionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<PromotionResponse>
                    {
                        Code = 1003,
                        Message = "Dữ liệu không hợp lệ",
                        Results = null
                    });
                }

                var response = await _promotionService.UpdatePromotionAsync(id, request);
                if (response == null)
                {
                    return NotFound(new ApiResponse<PromotionResponse>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy chương trình khuyến mãi",
                        Results = null
                    });
                }

                return Ok(new ApiResponse<PromotionResponse>
                {
                    Code = 1000,
                    Message = "Cập nhật khuyến mãi thành công",
                    Results = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PromotionResponse>
                {
                    Code = 9999,
                    Message = $"Lỗi server: {ex.Message}",
                    Results = null
                });
            }
        }

        // DELETE: api/Promotion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeletePromotion(int id)
        {
            try
            {
                var result = await _promotionService.DeletePromotionAsync(id);
                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Code = 1002,
                        Message = "Không tìm thấy chương trình khuyến mãi",
                        Results = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = 1000,
                    Message = "Xóa chương trình khuyến mãi thành công",
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
    }
}
