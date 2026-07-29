using System.ComponentModel.DataAnnotations;

namespace api.varietyshop.com.tr.DTOs.Admin
{
    public class BalanceOverrideDto
    {
        [Required]
        public decimal NewBalance { get; set; }
        public string Reason { get; set; } = string.Empty; // Loglarda görünmesi için sebep
    }

    public class TrustScoreDto
    {
        [Required, Range(0, 100)]
        public decimal NewScore { get; set; }
    }

    public class SuspendUserDto
    {
        [Required]
        public bool IsSuspended { get; set; } // true ise banla, false ise banı aç
        public string Reason { get; set; } = string.Empty;
    }
    public class AddBoruUserDto
    {
        public string TargetUsername { get; set; } = string.Empty;
        public string Role { get; set; } = "Börü"; // "Börü" veya "Börücük"
        public int DurationDays { get; set; } = 30; // Börü için eklenecek gün sayısı
        public string? Note { get; set; } // Opsiyonel müşteri notu
    }

    public class AddBoruClanDto
    {
        public string TargetClanName { get; set; } = string.Empty;
        // 🔥 İŞTE EKSİK OLAN SATIR BU! 🔥
        public int DurationDays { get; set; } // Klanın kaç gün VIP olacağı (0 ise sınırsız)
    }
}