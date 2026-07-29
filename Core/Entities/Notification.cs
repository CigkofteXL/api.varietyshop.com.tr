using System;
using System.ComponentModel.DataAnnotations;

namespace api.varietyshop.com.tr.Core.Entities // Kendi namespace'ine göre ayarla
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; } // Bildirim kime gitti
        public Guid? ProductId { get; set; } // 🚨 YENİ: Yorum yapabilmek için ürünün ID'sini tutuyoruz

        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } // "success", "info", "warning", "error" (React'teki renklendirme için)

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}