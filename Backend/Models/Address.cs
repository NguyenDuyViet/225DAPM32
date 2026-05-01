namespace Backend.Models
{
    public class Address
    {
        public int IdAddress { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string AddressDetail { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string Note { get; set; }
        public bool IsDefault { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}