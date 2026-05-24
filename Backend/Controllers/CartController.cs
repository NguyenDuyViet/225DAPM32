using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<CartResponse>>> GetMyCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartByUserIdAsync(userId);

            return Ok(new ApiResponse<CartResponse>
            {
                Code = 1000,
                Message = "Lấy giỏ hàng thành công",
                Results = cart
            });
        }

        [HttpPost("items")]
        public async Task<ActionResult<ApiResponse<CartResponse>>> AddItem([FromBody] CartItemRequest request)
        {
            try
            {
                var userId = GetUserId();
                var cart = await _cartService.AddItemAsync(userId, request);

                return Ok(new ApiResponse<CartResponse>
                {
                    Code = 1000,
                    Message = "Thêm món vào giỏ hàng thành công",
                    Results = cart
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ToError<CartResponse>(1002, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ToError<CartResponse>(1003, ex.Message));
            }
        }

        [HttpPut("items/{idCartFood}")]
        public async Task<ActionResult<ApiResponse<CartResponse>>> UpdateItem(int idCartFood, [FromBody] UpdateCartItemRequest request)
        {
            try
            {
                var userId = GetUserId();
                var cart = await _cartService.UpdateItemAsync(userId, idCartFood, request);

                return Ok(new ApiResponse<CartResponse>
                {
                    Code = 1000,
                    Message = "Cập nhật giỏ hàng thành công",
                    Results = cart
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ToError<CartResponse>(1002, ex.Message));
            }
        }

        [HttpDelete("items/{idCartFood}")]
        public async Task<ActionResult<ApiResponse<CartResponse>>> RemoveItem(int idCartFood)
        {
            try
            {
                var userId = GetUserId();
                var cart = await _cartService.RemoveItemAsync(userId, idCartFood);

                return Ok(new ApiResponse<CartResponse>
                {
                    Code = 1000,
                    Message = "Xóa món khỏi giỏ hàng thành công",
                    Results = cart
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ToError<CartResponse>(1002, ex.Message));
            }
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse<object>>> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearCartAsync(userId);

            return Ok(new ApiResponse<object>
            {
                Code = 1000,
                Message = "Xóa giỏ hàng thành công",
                Results = default!
            });
        }

        private int GetUserId()
        {
            var userIdValue = User.FindFirstValue("UserId");
            if (!int.TryParse(userIdValue, out var userId))
                throw new UnauthorizedAccessException("Token không hợp lệ.");

            return userId;
        }

        private static ApiResponse<T> ToError<T>(int code, string message)
        {
            return new ApiResponse<T>
            {
                Code = code,
                Message = message,
                Results = default!
            };
        }
    }
}
