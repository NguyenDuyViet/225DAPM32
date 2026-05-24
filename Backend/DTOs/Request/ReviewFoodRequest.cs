namespace Backend.DTOs.Request
{
    public class ReviewFoodRequest
    {
        public int IdOrder { get; set; }

        public int IdFood { get; set; }

        public float Rating { get; set; }

        public string? Comment { get; set; }

        public string? Image { get; set; }

        public string? Video { get; set; }
    }
}
