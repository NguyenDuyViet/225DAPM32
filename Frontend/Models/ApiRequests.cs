namespace _225DAPM32.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
        public DateTime? LastOnline { get; set; }
        public decimal? CurrentLat { get; set; }
        public decimal? CurrentLng { get; set; }
    }

    public class CreateShipperRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }

    public class CartItemRequest
    {
        public int IdFood { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Note { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }

    public class CreateOrderRequest
    {
        public string DeliveryAddress { get; set; } = string.Empty;
        public decimal? DeliveryLat { get; set; }
        public decimal? DeliveryLng { get; set; }
        public string PaymentMethod { get; set; } = "cash";
        public int? IdVoucher { get; set; }
        public decimal ShippingFee { get; set; } = 15000m;
        public string? Note { get; set; }
    }
}
