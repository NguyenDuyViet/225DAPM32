namespace Backend.Models
{
    public class Review
    {
        public int IdReview { get; set; }
        public int IdUser { get; set; }
        public int IdOrder { get; set; }
        public int IdRestaurant { get; set; }
        public float FoodRating { get; set; }
        public float DriverRating { get; set; }
        public string CommentForRes { get; set; }
        public string CommentForShipper { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Order Order { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<ReviewFood> ReviewFoods { get; set; }
    }
}