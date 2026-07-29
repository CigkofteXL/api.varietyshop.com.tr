namespace api.varietyshop.com.tr.Core.Entities
{
    public class UserAsset
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } // Satın alan ajan
        public string ProductName { get; set; } = string.Empty; // Örn: Börü Hayalet Paket
        public string KeyData { get; set; } = string.Empty; // Teslim edilen lisans/kod
        public DateTime SoldAt { get; set; } = DateTime.UtcNow;
    }
}