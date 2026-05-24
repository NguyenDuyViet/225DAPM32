namespace Backend.Models
{
    public class ReviewFood
    {
        public int IdReviewFood { get; set; }
        public int IdReview { get; set; }
        public int IdFood { get; set; }
        public float Rating { get; set; }
        public string? Comment { get; set; }
        public string? Image { get; set; }
        public string? Video { get; set; }

        // Navigation properties
        public Review Review { get; set; }
        public Food Food { get; set; }
    }
}