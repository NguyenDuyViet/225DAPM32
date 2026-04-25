using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _225DAPM32.Models
{
    public class Address
    {
        [Key]
        public int Id_Address { get; set; }
        public int Id_User { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string AddressDetail { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string? Note { get; set; }
        public bool Is_Default { get; set; }

        [ForeignKey("Id_User")]
        public User? User { get; set; }
    }
}