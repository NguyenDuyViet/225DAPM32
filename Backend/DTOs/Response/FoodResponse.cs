namespace Backend.DTOs.Response
{
    public class FoodResponse
    {
        public int IdFood { get; set; }
        public int IdCategory { get; set; }
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public int? CookCount { get; set; }
        public int? PrepTime { get; set; }
        public int DailyQuantity { get; set; }
        public int SoldQuantity { get; set; }
        
        // Optional: Thông tin bổ sung
        public string? CategoryName { get; set; }
        public string? RestaurantName { get; set; }
    }
}
