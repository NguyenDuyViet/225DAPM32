namespace _225DAPM32.Areas.Admin.Models
{
    public class AdminDashboardViewModel
    {
        public int RestaurantsCount { get; set; }
        public int UsersCount { get; set; }
        public int OrdersToday { get; set; }
        public List<_225DAPM32.Models.Order> RecentOrders { get; set; } = new();
        public List<_225DAPM32.Models.Restaurant> RecentRestaurants { get; set; } = new();
        public List<string> StatusLabels { get; set; } = new();
        public List<int> StatusData { get; set; } = new();
    }
}
