using Backend.DTOs.Response;
using Backend.DTOs.Request;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders/restaurant/{restaurantId}
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Order>>>> GetOrdersByRestaurant(int restaurantId)
        {
            var orders = await _orderService.GetOrdersByRestaurantAsync(restaurantId);
            return Ok(new ApiResponse<IEnumerable<Order>>
            {
                Code = 200,
                Message = "Success",
                Results = orders
            });
        }

        // PUT: api/Orders/{id}/status
        [HttpPut("{id}/status")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, status);
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

        // PUT: api/Orders/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrder(int id, [FromBody] OrderRequest dto)
        {
            var result = await _orderService.UpdateOrderAsync(id, dto);
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

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
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

        // POST: api/Orders/seed
        [HttpPost("seed")]
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
                    Message = "Successfully seeded a new order for Bếp Nhà Việt (Restaurant ID 1)",
                    Results = new {
                        order.IdOrder,
                        order.OrderCode,
                        order.FinalTotal,
                        order.Status,
                        order.CreatedAt
                    }
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Code = 500,
                    Message = $"Error seeding order: {ex.Message}",
                    Results = ex.ToString()
                });
            }
        }
    }
}
