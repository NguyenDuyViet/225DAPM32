using Backend.DTOs.Response;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>Get chat history for a room (paginated)</summary>
        [HttpGet("history/{roomId}")]
        public async Task<ActionResult<ApiResponse<List<ChatMessage>>>> GetHistory(
            string roomId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var messages = await _chatService.GetRoomHistoryAsync(roomId, page, pageSize);
            return Ok(new ApiResponse<List<ChatMessage>>
            {
                Code = 200,
                Message = "Success",
                Results = messages
            });
        }

        /// <summary>Get list of active chat rooms for a restaurant</summary>
        [HttpGet("rooms/{restaurantId}")]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetRooms(int restaurantId)
        {
            var rooms = await _chatService.GetActiveRoomsAsync(restaurantId);
            return Ok(new ApiResponse<List<string>>
            {
                Code = 200,
                Message = "Success",
                Results = rooms
            });
        }

        /// <summary>Mark all messages in a room as read</summary>
        [HttpPost("rooms/{roomId}/read")]
        public async Task<IActionResult> MarkAsRead(string roomId, [FromQuery] string role = "restaurant")
        {
            await _chatService.MarkRoomAsReadAsync(roomId, role);
            return Ok(new { success = true });
        }

        /// <summary>Get unread count for a room</summary>
        [HttpGet("rooms/{roomId}/unread")]
        public async Task<ActionResult<ApiResponse<int>>> GetUnreadCount(string roomId, [FromQuery] string role = "restaurant")
        {
            var count = await _chatService.CountUnreadAsync(roomId, role);
            return Ok(new ApiResponse<int>
            {
                Code = 200,
                Message = "Success",
                Results = count
            });
        }
    }
}
