namespace _225DAPM32.Areas.Shipper.Models
{
    public class ShipperDashboardViewModel
    {
        public int ActiveDeliveries { get; set; }
        public int CompletedDeliveries { get; set; }
        public decimal TodayEarnings { get; set; }
        public decimal Rating { get; set; }
        public List<_225DAPM32.Models.Order> CurrentOrders { get; set; } = new();
    }
}
