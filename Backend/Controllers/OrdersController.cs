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
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetMyOrders()
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(GetUserId());

            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy danh sách đơn hàng thành công",
                Results = orders
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy danh sách đơn hàng thành công",
                Results = orders
            });
        }

        [HttpGet("restaurant/{idRestaurant}")]
        [Authorize(Roles = "admin,restaurant")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetOrdersByRestaurant(int idRestaurant)
        {
            var orders = await _orderService.GetOrdersByRestaurantAsync(idRestaurant);

            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy đơn hàng theo nhà hàng thành công",
                Results = orders
            });
        }

        [HttpGet("shipper/{idDriver}")]
        [Authorize(Roles = "admin,shipper")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetOrdersByShipper(int idDriver, [FromQuery] bool history = false)
        {
            var orders = await _orderService.GetOrdersByDriverAsync(idDriver, history);

            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy đơn hàng theo shipper thành công",
                Results = orders
            });
        }

        [HttpGet("{idOrder}")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> GetOrder(int idOrder)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(GetUserId(), idOrder);

                return Ok(new ApiResponse<OrderResponse>
                {
                    Code = 1000,
                    Message = "Lấy thông tin đơn hàng thành công",
                    Results = order
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ToError<OrderResponse>(1002, ex.Message));
            }
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> Checkout([FromBody] CreateOrderRequest request)
        {
            try
            {
                var order = await _orderService.CreateOrderFromCartAsync(GetUserId(), request);

                return CreatedAtAction(nameof(GetOrder), new { idOrder = order.IdOrder }, new ApiResponse<OrderResponse>
                {
                    Code = 1000,
                    Message = "Tạo đơn hàng thành công",
                    Results = order
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ToError<OrderResponse>(1003, ex.Message));
            }
        }

        [HttpPut("{idOrder}/cancel")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> CancelOrder(int idOrder, [FromBody] CancelOrderRequest request)
        {
            try
            {
                var order = await _orderService.CancelOrderAsync(GetUserId(), idOrder, request);

                return Ok(new ApiResponse<OrderResponse>
                {
                    Code = 1000,
                    Message = "Hủy đơn hàng thành công",
                    Results = order
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ToError<OrderResponse>(1002, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ToError<OrderResponse>(1003, ex.Message));
            }
        }

        [HttpPut("{idOrder}/status")]
        [Authorize(Roles = "admin,restaurant,shipper")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> UpdateOrderStatus(int idOrder, [FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(idOrder, request);

                return Ok(new ApiResponse<OrderResponse>
                {
                    Code = 1000,
                    Message = "Cập nhật trạng thái đơn hàng thành công",
                    Results = order
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ToError<OrderResponse>(1002, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ToError<OrderResponse>(1003, ex.Message));
            }
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
