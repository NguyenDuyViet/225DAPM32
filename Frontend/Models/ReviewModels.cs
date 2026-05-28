namespace _225DAPM32.Models
{
    public class RestaurantReview
    {
        public int IdReview { get; set; }
        public float FoodRating { get; set; }
        public float DriverRating { get; set; }
        public string? CommentForRes { get; set; }
        public string? CommentForShipper { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CustomerName { get; set; }
    }

    public class FoodReview
    {
        public string Username { get; set; } = string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;
        public float Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SubmitOrderReviewRequest
    {
        public int IdOrder { get; set; }
        public float RestaurantRating { get; set; }
        public float DriverRating { get; set; }
        public string? CommentForRestaurant { get; set; }
        public string? CommentForShipper { get; set; }
        public List<SubmitFoodReviewRequest> Foods { get; set; } = new();
    }

    public class SubmitFoodReviewRequest
    {
        public int IdFood { get; set; }
        public float Rating { get; set; }
        public string? Comment { get; set; }
    }
}
