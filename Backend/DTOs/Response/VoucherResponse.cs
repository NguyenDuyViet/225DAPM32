namespace Backend.DTOs.Response
{
    public class VoucherResponse
    {
        public int IdVoucher { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime Expiry { get; set; }
    }
}
