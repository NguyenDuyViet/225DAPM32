namespace Backend.Models
{
    public class Notification
    {
        public int IdNoti { get; set; }
        public int IdUser { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int? OrderId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}