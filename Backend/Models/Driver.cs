namespace Backend.Models
{
    public class Driver
    {
        public int IdDriver { get; set; }
        public int IdUser { get; set; }
        public string LicensePlate { get; set; }
        public string Address { get; set; }
        public string ExpRank { get; set; }
        public string DescStatus { get; set; }
        public decimal CurrentLat { get; set; }
        public decimal CurrentLng { get; set; }
        public bool IsBusy { get; set; }
        public decimal RateAvg { get; set; }
        public int TotalOrders { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}