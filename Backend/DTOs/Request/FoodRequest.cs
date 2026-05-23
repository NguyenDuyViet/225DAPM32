namespace Backend.DTOs.Request
{
    public class FoodRequest
    {
        public int IdCategory { get; set; }
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; } = 0;
        public int? PrepTime { get; set; }
        public int DailyQuantity { get; set; } = 50;
    }
}
