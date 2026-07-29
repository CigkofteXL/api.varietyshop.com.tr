using System;

namespace api.varietyshop.com.tr.Core.Entities
{
    public class ExchangeRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Talebi açan ajanın ID'si ve ilişkisi
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string CodeOrItem { get; set; } = string.Empty; // Gönderilen E-pin kodu veya hesap bilgisi
        public string Platform { get; set; } = string.Empty;   // Örn: Wolvesville, Valorant, Steam
        public decimal EstimatedValue { get; set; }           // Kullanıcının beklediği tahmini tutar
        public string Status { get; set; } = "Pending";        // Pending, Approved, Rejected
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}