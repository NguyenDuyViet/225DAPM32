namespace Backend.Models
{
    public class FoodImage
    {
        public int IdFoodimage { get; set; }
        public int IdFood { get; set; }
        public string Image { get; set; }

        // Navigation properties
        public Food Food { get; set; }
    }
}