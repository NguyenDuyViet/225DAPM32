namespace Backend.Models
{
    public class Restaurant
    {
        public int IdRestaurant { get; set; }
        public string NameRestaurant { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public ICollection<Food> Foods { get; set; }
        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
