using System;

namespace api.varietyshop.com.tr.Core.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty; // Örn: "Aylık VIP Üyelik" veya "1000 Wov Altını"
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } // Resmi satış fiyatı

        public int Stock { get; set; } = -1; // Sınırsız stok için -1 kullanacağız
        public string Category { get; set; } = "General"; // VIP, Gold, Pin, Boost
        public string? ImageUrl { get; set; } // Ürün resmi

        public bool IsActive { get; set; } = true; // Satışta mı?
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ==========================================
        // 🚀 SATIŞ VE DÖNÜŞÜM PATLATICILAR (YENİ)
        // ==========================================

        // 1. Flaş İndirim Sistemi
        public decimal? FlashSalePrice { get; set; } // İndirimli fiyat (Eğer null ise indirim yok)
        public DateTime? FlashSaleEndDate { get; set; } // İndirimin biteceği tarih ve saat

        // 2. Cashback (Para İadesi) Sistemi
        public decimal CashbackPercentage { get; set; } = 0; // % kaçı iade edilecek? Örn: 5 yazarsan %5'i bakiye olarak döner
    }
}