using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("food/{idFood}")]
        public async Task<ActionResult<ApiResponse<List<ReviewFoodResponse>>>> GetReviewFoodByIdFood(int idFood)
        {
            var reviews = await _reviewService.GetReviewFoodByIdFood(idFood);

            return Ok(new ApiResponse<List<ReviewFoodResponse>>
            {
                Code = 1000,
                Message = "Lấy danh sách đánh giá món ăn thành công",
                Results = reviews
            });
        }

        [Authorize]
        [HttpPost("food")]
        public async Task<ActionResult<ApiResponse<ReviewFoodResponse>>> CreateReviewFood([FromBody] ReviewFoodRequest request)
        {
            try
            {
                var review = await _reviewService.CreateReviewFood(request, GetUserId());

                return Ok(new ApiResponse<ReviewFoodResponse>
                {
                    Code = 1000,
                    Message = "Đánh giá món ăn thành công",
                    Results = review
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ReviewFoodResponse>
                {
                    Code = 1003,
                    Message = ex.Message,
                    Results = default!
                });
            }
        }

        private int GetUserId()
        {
            var userIdValue = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdValue, out var userId))
                throw new UnauthorizedAccessException("Token không hợp lệ.");

            return userId;
        }
    }
}
