namespace Backend.DTOs.Request
{
    public class OrderRequest
    {
        public string? DeliveryAddress { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
    }
}
