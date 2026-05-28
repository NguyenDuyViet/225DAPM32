namespace Backend.DTOs.Request
{
    public class OrderReviewRequest
    {
        public int IdOrder { get; set; }
        public float RestaurantRating { get; set; }
        public float DriverRating { get; set; }
        public string? CommentForRestaurant { get; set; }
        public string? CommentForShipper { get; set; }
        public List<OrderFoodReviewRequest> Foods { get; set; } = new();
    }

    public class OrderFoodReviewRequest
    {
        public int IdFood { get; set; }
        public float Rating { get; set; }
        public string? Comment { get; set; }
    }
}
