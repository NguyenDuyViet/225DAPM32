using System;

namespace _225DAPM32.Models
{
    public class Promotion
    {
        public int IdPromo { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public decimal MinOrderValue { get; set; }
        public decimal MaxDiscount { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IdRestaurant { get; set; }
    }
}
