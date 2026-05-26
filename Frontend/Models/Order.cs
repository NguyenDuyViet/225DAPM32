using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _225DAPM32.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public string OrderCode { get; set; }
        public int IdRestaurant { get; set; }
        public string RestaurantName { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal? DeliveryLat { get; set; }
        public decimal? DeliveryLng { get; set; }
        public int IdAddress { get; set; }
        public int? IdDriver { get; set; }
        public int? IdVoucher { get; set; }
        public string PaymentMethod { get; set; }
        public decimal FoodAmount { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalTotal { get; set; }
        public string? PaymentStatus { get; set; }
        public string Status { get; set; } // 'pending','confirmed','delivering','completed','canceled'
        public string? Note { get; set; }
        public string? CancelReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Legacy/frontend fields kept for existing customer profile/cart screens.
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? DeliveringAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public List<OrderItem> Items { get; set; } = new();

        // Additional frontend properties
        public string? TrackingNumber { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public DateTime? EstimatedDelivery { get; set; }

        [ForeignKey("IdUser")]
        public User? User { get; set; }
        [ForeignKey("IdAddress")]
        public Address? Address { get; set; }
        [ForeignKey("IdRestaurant")]
        public Restaurant? Restaurant { get; set; }

        public string StatusText => Status switch
        {
            "completed" => "Hoàn thành",
            "delivering" => "Đang giao",
            "confirmed" => "Đã xác nhận",
            "pending" => "Chờ xử lý",
            "canceled" => "Đã hủy",
            _ => "Không xác định"
        };

        public string StatusColor => Status switch
        {
            "completed" => "success",
            "delivering" => "warning",
            "confirmed" => "info",
            "pending" => "primary",
            "canceled" => "danger",
            _ => "secondary"
        };

        public string PaymentStatusText => PaymentStatus switch
        {
            "paid" => "Đã thanh toán",
            "unpaid" => "Chưa thanh toán",
            "refunded" => "Đã hoàn tiền",
            _ => "Chưa xác định"
        };

        public List<OrderFoodViewModel>? OrderFoods { get; set; }
    }

    public class OrderItem
    {
        public int IdOrderFood { get; set; }
        public int IdFood { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string FoodImage { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
    }

    public class OrderFoodViewModel
    {
        public int IdOrderFood { get; set; }
        public int IdFood { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
    }
}
