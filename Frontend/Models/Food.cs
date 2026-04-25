using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _225DAPM32.Models
{
    public class Food
    {
        [Key]
        public int Id_Food { get; set; }
        public int Id_Category { get; set; }
        public int Id_Restaurant { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Cook_Count { get; set; }
        public int Prep_Time { get; set; }

        [ForeignKey("Id_Category")]
        public Category Category { get; set; }
        [ForeignKey("Id_Restaurant")]
        public Restaurant Restaurant { get; set; }
    }
}