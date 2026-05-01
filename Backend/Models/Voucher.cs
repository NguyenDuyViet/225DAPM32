namespace Backend.Models
{
    public class Voucher
    {
        public int IdVoucher { get; set; }
        public string Code { get; set; }
        public int IdUser { get; set; }
        public decimal Value { get; set; }
        public DateTime Expiry { get; set; }
        public bool Used { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}