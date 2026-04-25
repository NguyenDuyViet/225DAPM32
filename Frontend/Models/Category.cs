using System.ComponentModel.DataAnnotations;

namespace _225DAPM32.Models
{
    public class Category
    {
        [Key]
        public int Id_Category { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}