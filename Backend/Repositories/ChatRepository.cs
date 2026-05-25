using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> GetMessagesByRoomAsync(string roomId, int skip = 0, int take = 50)
        {
            return await _context.ChatMessages
                .Where(m => m.RoomId == roomId)
                .OrderBy(m => m.SentAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<string>> GetActiveRoomsForRestaurantAsync(int restaurantId)
        {
            var prefix = $"order_";
            // Get rooms that belong to this restaurant's orders
            return await _context.ChatMessages
                .Where(m => m.RoomId.StartsWith(prefix))
                .Select(m => m.RoomId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetLastMessagePerRoomAsync(List<string> roomIds)
        {
            var messages = new List<ChatMessage>();
            foreach (var roomId in roomIds)
            {
                var last = await _context.ChatMessages
                    .Where(m => m.RoomId == roomId)
                    .OrderByDescending(m => m.SentAt)
                    .FirstOrDefaultAsync();
                if (last != null) messages.Add(last);
            }
            return messages;
        }

        public async Task<int> CountUnreadAsync(string roomId, string receiverRole)
        {
            return await _context.ChatMessages
                .CountAsync(m => m.RoomId == roomId && !m.IsRead && m.SenderRole != receiverRole);
        }

        public async Task MarkRoomAsReadAsync(string roomId, string receiverRole)
        {
            var unread = await _context.ChatMessages
                .Where(m => m.RoomId == roomId && !m.IsRead && m.SenderRole != receiverRole)
                .ToListAsync();
            unread.ForEach(m => m.IsRead = true);
            await _context.SaveChangesAsync();
        }

        public async Task<ChatMessage> SaveMessageAsync(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
