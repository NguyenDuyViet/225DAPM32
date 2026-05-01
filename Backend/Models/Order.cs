namespace Backend.Models
{
    public class Order
    {
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public int IdAddress { get; set; }
        public int? IdDriver { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
        public string Status { get; set; } // 'pending','confirmed','delivering','completed','canceled'
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? DeliveringAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CanceledAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Address Address { get; set; }
        public Driver Driver { get; set; }
        public ICollection<OrderPromotion> OrderPromotions { get; set; }
        public OrderRestaurant OrderRestaurant { get; set; }
        public ICollection<OrderFood> OrderFoods { get; set; }
        public Review Review { get; set; }
        public PaymentMethod PaymentMethodEntity { get; set; }
        public Complaint Complaint { get; set; }
    }
}