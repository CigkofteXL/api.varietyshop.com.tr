namespace api.varietyshop.com.tr.DTOs.Webhooks
{
    public class DiscordPayloadDto
    {
        public string content { get; set; } = string.Empty; // Discord "content" kelimesini küçük harfle bekler!
        public string username { get; set; } = "VarietyShop Sistem Börü"; // Bota isim veriyoruz
        public string avatar_url { get; set; } = "https://i.imgur.com/4M34hi2.png"; // Havalı bir logo koyabilirsin
    }
}