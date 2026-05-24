namespace Backend.DTOs.Response
{
    public class ReviewFoodResponse
    {
        public string Username { get; set; } = string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public string FoodName { get; set; } = string.Empty;
        public float Rating { get; set; }
        public string? Comment { get; set; }
        public string? Image { get; set; }
        public string? Video { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
