using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class ChatService
    {
        private readonly ChatRepository _chatRepository;

        public ChatService(ChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<List<ChatMessage>> GetRoomHistoryAsync(string roomId, int page = 1, int pageSize = 50)
        {
            int skip = (page - 1) * pageSize;
            return await _chatRepository.GetMessagesByRoomAsync(roomId, skip, pageSize);
        }

        public async Task<ChatMessage> SaveMessageAsync(ChatMessage message)
        {
            message.SentAt = DateTime.UtcNow;
            return await _chatRepository.SaveMessageAsync(message);
        }

        public async Task<List<string>> GetActiveRoomsAsync(int restaurantId)
        {
            return await _chatRepository.GetActiveRoomsForRestaurantAsync(restaurantId);
        }

        public async Task MarkRoomAsReadAsync(string roomId, string receiverRole)
        {
            await _chatRepository.MarkRoomAsReadAsync(roomId, receiverRole);
        }

        public async Task<int> CountUnreadAsync(string roomId, string receiverRole)
        {
            return await _chatRepository.CountUnreadAsync(roomId, receiverRole);
        }
    }
}
