using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _225DAPM32.Models
{
    public class CartFood
    {
        [Key]
        public int IdCartFood { get; set; }
        public int IdCart { get; set; }
        public int IdFood { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }

        [ForeignKey("IdCart")]
        public Cart Cart { get; set; }
        [ForeignKey("IdFood")]
        public Food Food { get; set; }
    }
}