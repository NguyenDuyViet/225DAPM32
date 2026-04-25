using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _225DAPM32.Models
{
    public class Order
    {
        [Key]
        public int Id_Order { get; set; }
        public int Id_User { get; set; }
        public int? Id_Address { get; set; }
        public int? Id_Driver { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
        public string Status { get; set; } // pending, confirmed, delivering, completed, canceled
        public string? Note { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime? Confirmed_At { get; set; }
        public DateTime? Delivering_At { get; set; }
        public DateTime? Delivered_At { get; set; }
        public DateTime? Canceled_At { get; set; }
        public string? TrackingNumber { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public DateTime? EstimatedDelivery { get; set; }

        [ForeignKey("Id_User")]
        public User? User { get; set; }
        [ForeignKey("Id_Address")]
        public Address? Address { get; set; }
    }
}