namespace api.varietyshop.com.tr.DTOs.Admin
{
    // DTO: Veri Taşıma Objesi (Aynı dosyanın en altına koyabilirsin)
    public class CreateAnnouncementDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = "Info"; // Arayüzdeki renklendirme için Warning/Update/Info
    }
}
