namespace api.varietyshop.com.tr.Core.Entities
{
    public class LicenseKey
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; } // Hangi ürüne ait? (Börü V1, Proxy vs.)
        public string KeyData { get; set; } = string.Empty; // Asıl lisans kodu veya dosya linki
        public bool IsSold { get; set; } = false; // Satıldı mı?
        public Guid? SoldToUserId { get; set; } // Kime satıldı?
        public DateTime? SoldAt { get; set; }

        // Bağlantı
        public virtual Product Product { get; set; } = null!;
    }
}
