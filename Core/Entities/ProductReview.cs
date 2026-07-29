using System;

namespace api.varietyshop.com.tr.Core.Entities
{
    public class ProductReview
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Hangi ürüne yapıldı?
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        // Kim yaptı?
        public Guid UserId { get; set; }
        public User User { get; set; }

        // Yorum ve Puan
        public int Rating { get; set; } // 1 ile 5 arası yıldız
        public string Comment { get; set; } = string.Empty;

        // Güvenlik ve Onay Mekanizması
        public bool IsApproved { get; set; } = false; // Admin onaylamadan sitede gözükmesin (Spam/Bot koruması)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}