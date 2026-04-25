using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _225DAPM32.Models
{
    public class Cart_Food
    {
        [Key]
        public int Id_CartFood { get; set; }
        public int Id_Cart { get; set; }
        public int Id_Food { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }

        [ForeignKey("Id_Cart")]
        public Cart Cart { get; set; }
        [ForeignKey("Id_Food")]
        public Food Food { get; set; }
    }
}