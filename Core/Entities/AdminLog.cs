namespace api.varietyshop.com.tr.Core.Entities
{
    public class AdminLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string AdminName { get; set; } = string.Empty; // İşlemi Yapan
        public string Action { get; set; } = string.Empty; // Ne Yaptı?
        public string Target { get; set; } = string.Empty; // Kime Yaptı?
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
