namespace _225DAPM32.Areas.Shipper.Models
{
    public class ShipperEarningsViewModel
    {
        public decimal WeekTotal { get; set; }
        public decimal TodayTotal { get; set; }
        public int CompletedOrders { get; set; }
        public int PendingOrders { get; set; }
    }
}