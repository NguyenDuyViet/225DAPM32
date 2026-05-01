namespace Backend.Models
{
    public class PaymentMethod
    {
        public int IdTransaction { get; set; }
        public int IdUser { get; set; }
        public int IdOrder { get; set; }
        public string Method { get; set; }
        public decimal Amount { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Order Order { get; set; }
    }
}