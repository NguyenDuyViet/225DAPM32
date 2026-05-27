namespace Backend.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public int IdRole { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastOnline { get; set; }
        public decimal? CurrentLat { get; set; }
        public decimal? CurrentLng { get; set; }
        public float? CancelRate { get; set; }

        // Navigation properties
        public Role Role { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Voucher> Vouchers { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Driver Driver { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<SystemLog> SystemLogs { get; set; }
    }
}
