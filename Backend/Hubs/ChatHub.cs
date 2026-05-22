using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        private readonly ILogger<ChatHub> _logger;

        // Track who is in which rooms: connectionId -> list of roomIds
        private static readonly Dictionary<string, HashSet<string>> _connectionRooms
            = new Dictionary<string, HashSet<string>>();

        public ChatHub(ChatService chatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        /// <summary>Client calls this to join a chat room and receive its history</summary>
        public async Task JoinRoom(string roomId, int senderId, string senderRole)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            // Track rooms for this connection
            lock (_connectionRooms)
            {
                if (!_connectionRooms.ContainsKey(Context.ConnectionId))
                    _connectionRooms[Context.ConnectionId] = new HashSet<string>();
                _connectionRooms[Context.ConnectionId].Add(roomId);
            }

            // Mark messages as read
            await _chatService.MarkRoomAsReadAsync(roomId, senderRole);

            // Send chat history to the caller only
            var history = await _chatService.GetRoomHistoryAsync(roomId);
            await Clients.Caller.SendAsync("LoadHistory", history);

            _logger.LogInformation("User {SenderId} ({SenderRole}) joined room {RoomId}", senderId, senderRole, roomId);
        }

        /// <summary>Client leaves a room</summary>
        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            lock (_connectionRooms)
            {
                if (_connectionRooms.ContainsKey(Context.ConnectionId))
                    _connectionRooms[Context.ConnectionId].Remove(roomId);
            }
        }

        /// <summary>Send a message to a room – saves to DB and broadcasts to all group members</summary>
        public async Task SendMessage(string roomId, string content, int senderId, string senderRole, string senderName, int? orderId = null)
        {
            if (string.IsNullOrWhiteSpace(content)) return;

            var message = new ChatMessage
            {
                RoomId = roomId,
                SenderId = senderId,
                SenderRole = senderRole,
                SenderName = senderName,
                OrderId = orderId,
                Content = content.Trim(),
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            // Persist to database
            var saved = await _chatService.SaveMessageAsync(message);

            // Broadcast to everyone in the room (including sender)
            await Clients.Group(roomId).SendAsync("ReceiveMessage", new
            {
                idMessage = saved.IdMessage,
                roomId = saved.RoomId,
                senderId = saved.SenderId,
                senderRole = saved.SenderRole,
                senderName = saved.SenderName,
                content = saved.Content,
                sentAt = saved.SentAt.ToString("o"),
                isRead = saved.IsRead,
                orderId = saved.OrderId
            });
        }

        /// <summary>Notify room that someone is typing</summary>
        public async Task Typing(string roomId, string senderName)
        {
            await Clients.OthersInGroup(roomId).SendAsync("UserTyping", senderName);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            lock (_connectionRooms)
            {
                _connectionRooms.Remove(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
