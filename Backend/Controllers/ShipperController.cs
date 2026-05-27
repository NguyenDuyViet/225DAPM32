using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin,shipper")]
    [Route("api/[controller]")]
    public class ShipperController : ControllerBase
    {
        private readonly OrderService _orderService;

        public ShipperController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetMyCurrentOrders()
        {
            var orders = await _orderService.GetOrdersByDriverUserIdAsync(GetUserId(), history: false);

            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy danh sách đơn đang giao thành công",
                Results = orders
            });
        }

        [HttpGet("history")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetMyHistory()
        {
            var orders = await _orderService.GetOrdersByDriverUserIdAsync(GetUserId(), history: true);

            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy lịch sử giao hàng thành công",
                Results = orders
            });
        }

        [HttpGet("{idDriver}/orders")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetCurrentOrders(int idDriver)
        {
            var orders = await _orderService.GetOrdersByDriverAsync(idDriver, history: false);

            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy danh sách đơn đang giao thành công",
                Results = orders
            });
        }

        [HttpGet("{idDriver}/history")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetHistory(int idDriver)
        {
            var orders = await _orderService.GetOrdersByDriverAsync(idDriver, history: true);

            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy lịch sử giao hàng thành công",
                Results = orders
            });
        }

        [HttpPut("orders/{idOrder}/delivering")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> MarkDelivering(int idOrder, [FromBody] UpdateOrderStatusRequest request)
        {
            request.Status = "delivering";
            return await UpdateMyStatus(idOrder, request, "Cập nhật đơn đang giao thành công");
        }

        [HttpPut("orders/{idOrder}/completed")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> MarkCompleted(int idOrder, [FromBody] UpdateOrderStatusRequest request)
        {
            request.Status = "completed";
            return await UpdateMyStatus(idOrder, request, "Hoàn tất đơn hàng thành công");
        }

        private async Task<ActionResult<ApiResponse<OrderResponse>>> UpdateMyStatus(int idOrder, UpdateOrderStatusRequest request, string message)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusForDriverAsync(GetUserId(), idOrder, request);

                return Ok(new ApiResponse<OrderResponse>
                {
                    Code = 1000,
                    Message = message,
                    Results = order
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<OrderResponse>
                {
                    Code = 1002,
                    Message = ex.Message,
                    Results = default!
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<OrderResponse>
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
                throw new UnauthorizedAccessException("Token khong hop le.");

            return userId;
        }
    }
}
