namespace Backend.Models
{
    public class Cart
    {
        public int IdCart { get; set; }
        public int IdUser { get; set; }
        public int Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<CartFood> CartFoods { get; set; }
    }
}