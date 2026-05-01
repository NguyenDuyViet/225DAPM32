namespace Backend.Models
{
    public class Complaint
    {
        public int IdComplaint { get; set; }
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public string Type { get; set; } // 'food','driver','other'
        public string Description { get; set; }
        public string Image { get; set; }
        public string Status { get; set; } // 'open','closed'
        public string HandledBy { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public User User { get; set; }
    }
}