namespace Backend.Models
{
    public class CartFood
    {
        public int IdCartFood { get; set; }
        public int IdCart { get; set; }
        public int IdFood { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Note { get; set; }

        // Navigation properties
        public Cart Cart { get; set; }
        public Food Food { get; set; }
    }
}