using System;
using System.Collections.Generic;

namespace api.varietyshop.com.tr.Core.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Talebi kim açtı?
        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public string Subject { get; set; } = string.Empty;

        public string Status { get; set; } = "Open";

        // 🚨 EKLENDİ: Biletin Önceliği
        public string Priority { get; set; } = "Normal";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TicketReply> Replies { get; set; } = new List<TicketReply>();
    }

    public class TicketReply
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;

        // Mesajı kim yazdı?
        public Guid SenderId { get; set; }
        public User Sender { get; set; } = null!;

        // 🚨 EKLENDİ: Bu mesajı yazan Admin miydi?
        public bool IsAdmin { get; set; } = false;

        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}