namespace Backend.Models
{
    public class OrderFood
    {
        public int IdOrderFood { get; set; }
        public int IdOrder { get; set; }
        public int IdFood { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }

        public Order Order { get; set; }
        public Food Food { get; set; }
    }
}
