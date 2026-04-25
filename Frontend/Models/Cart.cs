using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _225DAPM32.Models
{
    public class Cart
    {
        [Key]
        public int Id_Cart { get; set; }
        public int Id_User { get; set; }
        public int Total { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Update_At { get; set; }

        [ForeignKey("Id_User")]
        public User User { get; set; }
    }
}