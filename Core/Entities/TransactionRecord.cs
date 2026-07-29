namespace api.varietyshop.com.tr.Core.Entities
{
    public class TransactionRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public decimal Amount { get; set; } // Tutar
        public string Type { get; set; } = string.Empty; // Deposit, Withdraw, NexusPurchase, Refund
        public string Status { get; set; } = "Completed"; // Pending, Completed, Failed
        public string? Description { get; set; } // Örn: "İtemsatış'tan bakiye yüklendi" VEYA "Nexus'tan hesap alındı"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
