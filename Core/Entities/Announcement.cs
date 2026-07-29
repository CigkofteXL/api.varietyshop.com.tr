namespace api.varietyshop.com.tr.Core.Entities
{
    public class Announcement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = "Info"; // "Info", "Warning", "Update"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}