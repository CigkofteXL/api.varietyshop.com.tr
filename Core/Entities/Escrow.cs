namespace api.varietyshop.com.tr.Core.Entities
{
    public class Escrow
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid BuyerId { get; set; } // Alıcı
        public Guid SellerId { get; set; } // Satıcı
        public Guid ListingId { get; set; } // Satılan İlan (NexusListing tablosuna bağlanacak)

        public decimal Amount { get; set; } // Havuzda kilitli tutulan para
        public string Status { get; set; } = "Holding"; // Holding (Bekliyor), Released (Satıcıya Aktarıldı), Disputed (Kavgalı/İtiraz)
        public string? DisputeReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReleasedAt { get; set; }
        // --- EKSİK OLAN KISIM BURASI (Navigation Properties) ---
        public virtual User Seller { get; set; } = null!;
        public virtual User Buyer { get; set; } = null!;
        // ====================================================================
        // 🚨 YENİ EKLENEN MANTIKSAL ZAMAN DAMGALARI
        // ====================================================================
        public DateTime? ShippedAt { get; set; }   // Satıcı "Ürünü teslim ettim" dediği an (24 saatlik sayaç başlangıcı)
        public DateTime? CompletedAt { get; set; } // Soru işareti koyduk çünkü ilk oluştuğunda boş olabilir
    }
}
