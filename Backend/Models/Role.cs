namespace Backend.Models
{
    public class Role
    {
        public int IdRole { get; set; }
        public string Name { get; set; } // 'customer', 'admin', 'restaurant', 'shipper'
        public string Description { get; set; }

        // Navigation properties
        public ICollection<User> Users { get; set; }
    }
}