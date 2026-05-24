namespace Backend.DTOs.Request
{
    public class CartItemRequest
    {
        public int IdFood { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Note { get; set; }
    }
}
