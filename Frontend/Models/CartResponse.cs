namespace _225DAPM32.Models
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

    public class CartItemResponse
    {
        public int IdCartFood { get; set; }
        public int IdFood { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string FoodImage { get; set; } = string.Empty;
        public int IdRestaurant { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal UnitPrice
        {
            get => Price;
            set => Price = value;
        }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPrice
        {
            get => Total;
            set => Total = value;
        }
        public string? Note { get; set; }
    }
}
