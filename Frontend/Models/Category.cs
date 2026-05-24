using System.ComponentModel.DataAnnotations;

namespace _225DAPM32.Models
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int? FoodCount { get; set; }
    }
}
