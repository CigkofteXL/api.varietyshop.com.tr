namespace api.varietyshop.com.tr.DTOs.Economy
{
    public class GenerateCouponDto
    {
        public string Code { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int MaxUsage { get; set; } = 1;
    }
}
