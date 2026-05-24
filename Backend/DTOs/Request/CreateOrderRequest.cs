namespace Backend.DTOs.Request
{
    public class CreateOrderRequest
    {
        public string DeliveryAddress { get; set; } = string.Empty;
        public decimal? DeliveryLat { get; set; }
        public decimal? DeliveryLng { get; set; }
        public string PaymentMethod { get; set; } = "cash";
        public int? IdVoucher { get; set; }
        public decimal ShippingFee { get; set; } = 15000m;
        public string? Note { get; set; }
    }
}
