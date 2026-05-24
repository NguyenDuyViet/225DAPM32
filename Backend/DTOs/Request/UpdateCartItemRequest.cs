namespace Backend.DTOs.Request
{
    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }
}
