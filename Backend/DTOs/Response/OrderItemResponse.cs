namespace Backend.DTOs.Response
{
    public class OrderItemResponse
    {
        public int IdOrderFood { get; set; }
        public int IdFood { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string FoodImage { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
    }
}
