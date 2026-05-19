namespace Backend.DTOs.Response
{
    public class CategoryResponse
    {
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int? FoodCount { get; set; }
    }
}
