namespace Backend.Models
{
    public class ChatMessage
    {
        public int IdMessage { get; set; }

        /// <summary>Room identifier: "order_{orderId}" or "r{restaurantId}_d{driverId}"</summary>
        public string RoomId { get; set; } = string.Empty;

        public int SenderId { get; set; }

        /// <summary>"restaurant" | "customer" | "driver"</summary>
        public string SenderRole { get; set; } = string.Empty;

        public string? SenderName { get; set; }

        public int? OrderId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}
