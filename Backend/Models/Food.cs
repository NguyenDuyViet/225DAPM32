namespace Backend.Models
{
    public class Food
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

        public Category Category { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<CartFood> CartFoods { get; set; }
        public ICollection<OrderFood> OrderFoods { get; set; }
        public ICollection<ReviewFood> ReviewFoods { get; set; }
    }
}
