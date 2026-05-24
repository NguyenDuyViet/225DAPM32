namespace Backend.DTOs.Response
{
    public class CartResponse
    {
        public int IdCart { get; set; }
        public int IdUser { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
    }
}
