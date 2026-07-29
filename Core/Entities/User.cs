namespace api.varietyshop.com.tr.Core.Entities
{
    public class User
    {
        // Temel Kimlik Bilgileri
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // User.cs sınıfının içine eklenecek o kritik iki alan kanka:
        public bool IsEmailVerified { get; set; } = false; // 🚨 Kayıt anında false başlar
        public string? EmailVerificationToken { get; set; } // 🚨 6 haneli siber kod burada saklanır

        // Profil ve Karargah Bilgileri
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Ekonomi ve Nexus (Karaborsa)
        public decimal Balance { get; set; } = 0.0m;

        // Rol ve Güvenlik Yönetimi
        public string Role { get; set; } = "User";
        public bool TwoFactorEnabled { get; set; } = false;
        public bool IsPublicProfile { get; set; } = true; // Profil dışarıdan görünür mü?
        public string? TwoFactorSecret { get; set; }

        // Davet Sistemi Sütunları
        public string? ReferralCode { get; set; } // Bu ajanın kendi davet kodu
        public Guid? ReferredById { get; set; }   // Bu ajanı kim davet etti? (Opsiyonel)

        // Börü Güvenlik Duvarı
        public string? HardwareId { get; set; }
        public string? LastKnownIp { get; set; }
        // 🚨 GÜVENLİK VE BAN SİSTEMİ
        public bool IsBanned { get; set; } = false; // Adam banlı mı?
        public string? BanReason { get; set; } // Neden banlandı? (Örn: Küfür, Çalıntı Kart)
    }
}
