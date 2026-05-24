namespace Backend.DTOs.Request
{
    public class UpdateOrderStatusRequest
    {
        public string Status { get; set; } = string.Empty;
        public int? IdDriver { get; set; }
        public string? CancelReason { get; set; }
    }
}
