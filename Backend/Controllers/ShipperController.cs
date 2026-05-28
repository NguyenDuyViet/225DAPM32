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
        private readonly UserService _userService;

        public ShipperController(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpPost("accounts")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> CreateAccount([FromBody] CreateShipperRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.LicensePlate))
            {
                return BadRequest(new ApiResponse<UserResponse>
                {
                    Code = 1003,
                    Message = "Vui long nhap day du tai khoan, email, mat khau va bien so xe.",
                    Results = default!
                });
            }

            var user = await _userService.CreateShipperAsync(request);
            if (user == null)
            {
                return BadRequest(new ApiResponse<UserResponse>
                {
                    Code = 1003,
                    Message = "Ten dang nhap hoac email da ton tai.",
                    Results = default!
                });
            }

            return Ok(new ApiResponse<UserResponse>
            {
                Code = 1000,
                Message = "Tao tai khoan shipper thanh cong.",
                Results = user
            });
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

        [HttpGet("available-orders")]
        public async Task<ActionResult<ApiResponse<List<OrderResponse>>>> GetAvailableOrders()
        {
            var orders = await _orderService.GetAvailableOrdersForDriverUserIdAsync(GetUserId());
            return Ok(new ApiResponse<List<OrderResponse>>
            {
                Code = 1000,
                Message = "Lấy danh sách đơn gần shipper thành công",
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

        [HttpPut("orders/{idOrder}/accept")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> AcceptOrder(int idOrder)
        {
            try
            {
                var order = await _orderService.AcceptOrderForDriverAsync(GetUserId(), idOrder);
                return Ok(new ApiResponse<OrderResponse>
                {
                    Code = 1000,
                    Message = "Nhận đơn thành công. Hãy đến nhà hàng lấy món.",
                    Results = order
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<OrderResponse> { Code = 1002, Message = ex.Message, Results = default! });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<OrderResponse> { Code = 1003, Message = ex.Message, Results = default! });
            }
        }

        [HttpPut("orders/{idOrder}/completed")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> MarkCompleted(int idOrder, [FromBody] UpdateOrderStatusRequest request)
        {
            request.Status = "completed";
            return await UpdateMyStatus(idOrder, request, "Hoàn tất đơn hàng thành công");
        }

        [HttpPut("location")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateLocation([FromBody] DriverLocationRequest request)
        {
            try
            {
                await _orderService.UpdateDriverLocationAsync(GetUserId(), request);
                return Ok(new ApiResponse<bool>
                {
                    Code = 1000,
                    Message = "Cập nhật vị trí shipper thành công",
                    Results = true
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Code = 1002,
                    Message = ex.Message,
                    Results = false
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Code = 1003,
                    Message = ex.Message,
                    Results = false
                });
            }
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
