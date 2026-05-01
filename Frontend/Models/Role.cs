using System.ComponentModel.DataAnnotations;

namespace _225DAPM32.Models
{
    public class Role
    {
        [Key]
        public int IdRole { get; set; }
        public string Name { get; set; } // 'customer', 'admin', 'restaurant', 'shipper'
        public string Description { get; set; }
    }
}