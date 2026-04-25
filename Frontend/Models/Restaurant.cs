using System.ComponentModel.DataAnnotations;

namespace _225DAPM32.Models
{
    public class Restaurant
    {
        [Key]
        public int Id_Restaurant { get; set; }
        public string Name_Restaurant { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}