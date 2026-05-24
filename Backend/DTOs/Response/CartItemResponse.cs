namespace Backend.DTOs.Response
{
    public class CartItemResponse
    {
        public int IdCartFood { get; set; }
        public int IdFood { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string FoodImage { get; set; } = string.Empty;
        public int IdRestaurant { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string? Note { get; set; }
    }
}
