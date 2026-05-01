namespace Backend.Models
{
    public class OrderRestaurant
    {
        public int IdOrder { get; set; }
        public int IdRestaurant { get; set; }
        public decimal ShipFee { get; set; }
        public string Status { get; set; } // 'pending','accepted','completed'

        // Navigation properties
        public Order Order { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}