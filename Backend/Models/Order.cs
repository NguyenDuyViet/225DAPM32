namespace Backend.Models
{
    public class Order
    {
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public int IdRestaurant { get; set; }
        public int? IdDriver { get; set; }
        public int? IdVoucher { get; set; }
        public string OrderCode { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal? DeliveryLat { get; set; }
        public decimal? DeliveryLng { get; set; }
        public string PaymentMethod { get; set; }
        public decimal FoodAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public string? CancelReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
        public Driver Driver { get; set; }
        public Voucher Voucher { get; set; }
        public ICollection<OrderPromotion> OrderPromotions { get; set; }
        public ICollection<OrderFood> OrderFoods { get; set; }
        public Review Review { get; set; }
        public PaymentMethod PaymentMethodEntity { get; set; }
        public Complaint Complaint { get; set; }
    }
}
