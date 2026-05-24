namespace Backend.DTOs.Response
{
    public class OrderResponse
    {
        public int IdOrder { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public int IdUser { get; set; }
        public int IdRestaurant { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public decimal? DeliveryLat { get; set; }
        public decimal? DeliveryLng { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal FoodAmount { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
        public string? CancelReason { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
    }
}
