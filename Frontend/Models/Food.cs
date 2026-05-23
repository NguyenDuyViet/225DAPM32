using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _225DAPM32.Models
{
    public class Food
    {
        [Key]
        public int IdFood { get; set; }
        public int IdCategory { get; set; }
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public int? CookCount { get; set; }
        public int? PrepTime { get; set; }
        public int DailyQuantity { get; set; }

        [ForeignKey("IdCategory")]
        public Category Category { get; set; }
        [ForeignKey("IdRestaurant")]
        public Restaurant Restaurant { get; set; }
    }
}