using System.Text.Json;
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
                Message = "Lay danh sach don hang thanh cong",
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
                Message = "Lay danh sach don hang thanh cong",
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
                Message = "Lay don hang theo nha hang thanh cong",
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
                Message = "Lay don hang theo shipper thanh cong",
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
                    Message = "Lay thong tin don hang thanh cong",
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
                    Message = "Tao don hang thanh cong",
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
                    Message = "Huy don hang thanh cong",
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
        public async Task<ActionResult<ApiResponse<OrderResponse>>> UpdateOrderStatus(int idOrder, [FromBody] JsonElement body)
        {
            try
            {
                var request = ParseStatusRequest(body);
                var order = await _orderService.UpdateOrderStatusAsync(idOrder, request);

                return Ok(new ApiResponse<OrderResponse>
                {
                    Code = 1000,
                    Message = "Cap nhat trang thai don hang thanh cong",
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

        [HttpPut("{idOrder}")]
        [Authorize(Roles = "admin,restaurant")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrder(int idOrder, [FromBody] OrderRequest dto)
        {
            var result = await _orderService.UpdateOrderAsync(idOrder, dto);
            if (!result)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Code = 404,
                    Message = "Order not found or update failed",
                    Results = false
                });
            }

            return Ok(new ApiResponse<bool>
            {
                Code = 200,
                Message = "Success",
                Results = true
            });
        }

        [HttpDelete("{idOrder}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteOrder(int idOrder)
        {
            var result = await _orderService.DeleteOrderAsync(idOrder);
            if (!result)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Code = 404,
                    Message = "Order not found or delete failed",
                    Results = false
                });
            }

            return Ok(new ApiResponse<bool>
            {
                Code = 200,
                Message = "Success",
                Results = true
            });
        }

        [HttpPost("seed")]
        [Authorize(Roles = "admin,restaurant")]
        public async Task<ActionResult<ApiResponse<object>>> SeedOrder()
        {
            try
            {
                var order = await _orderService.SeedOrderAsync();
                if (order == null)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Code = 400,
                        Message = "Failed to seed order",
                        Results = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = 200,
                    Message = "Successfully seeded a new order for restaurant",
                    Results = new
                    {
                        order.IdOrder,
                        order.OrderCode,
                        order.FinalTotal,
                        order.Status,
                        order.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Code = 500,
                    Message = $"Error seeding order: {ex.Message}",
                    Results = ex.ToString()
                });
            }
        }

        [HttpPost("{idOrder}/simulate-delivery-and-review")]
        [Authorize(Roles = "admin,restaurant")]
        public async Task<ActionResult<ApiResponse<object>>> SimulateDeliveryAndReview(int idOrder)
        {
            try
            {
                var result = await _orderService.SimulateDeliveryAndReviewAsync(idOrder);
                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Code = 404,
                        Message = "Order not found or simulation failed",
                        Results = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Code = 200,
                    Message = "Success",
                    Results = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Code = 500,
                    Message = $"Simulation error: {ex.Message}",
                    Results = ex.ToString()
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

        private static UpdateOrderStatusRequest ParseStatusRequest(JsonElement body)
        {
            if (body.ValueKind == JsonValueKind.String)
            {
                return new UpdateOrderStatusRequest
                {
                    Status = body.GetString() ?? string.Empty
                };
            }

            var request = body.Deserialize<UpdateOrderStatusRequest>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return request ?? new UpdateOrderStatusRequest();
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
