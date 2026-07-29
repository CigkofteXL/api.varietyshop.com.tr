using System;

namespace api.varietyshop.com.tr.Core.Entities
{
    public class Coupon
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; } = string.Empty; // Örn: BÖRU2026
        public decimal RewardAmount { get; set; } // Kodu girene kaç para verecek?

        public int MaxUsage { get; set; } = 1; // Kaç kişi kullanabilir?
        public int CurrentUsage { get; set; } = 0; // Şu ana kadar kaç kişi kullandı?

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
    }
}