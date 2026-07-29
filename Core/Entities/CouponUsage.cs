namespace api.varietyshop.com.tr.Core.Entities
{
    public class CouponUsage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CouponId { get; set; } // Hangi kupon?
        public Guid UserId { get; set; }   // Hangi ajan kullandı?
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties (Opsiyonel ama raporlama için iyi olur)
        public virtual User User { get; set; } = null!;
    }
}