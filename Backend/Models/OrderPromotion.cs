namespace Backend.Models
{
    public class OrderPromotion
    {
        public int IdOrder { get; set; }
        public int IdPromo { get; set; }
        public decimal DiscountAmount { get; set; }

        // Navigation properties
        public Order Order { get; set; }
        public Promotion Promotion { get; set; }
    }
}