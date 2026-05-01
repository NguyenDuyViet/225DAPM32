namespace Backend.Models
{
    public class Category
    {
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        // Navigation properties
        public ICollection<Food> Foods { get; set; }
    }
}