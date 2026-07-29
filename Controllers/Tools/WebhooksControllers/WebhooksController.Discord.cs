using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using api.varietyshop.com.tr.DTOs.Webhooks;

namespace api.varietyshop.com.tr.Controllers.Tools
{
    public partial class WebhooksController : ControllerBase
    {
        // URL: POST /tools/webhooks/discord/test-ping
        [HttpPost("discord/test-ping")]
        [Authorize(Roles = "Admin,Founder")] // Sadece sen test edebilirsin
        public async Task<IActionResult> TestPing()
        {
            var webhookUrl = _configuration["DiscordWebhooks:MarketAlerts"];
            if (string.IsNullOrEmpty(webhookUrl)) return BadRequest("Discord Webhook URL ayarlanmamış!");

            var payload = new DiscordPayloadDto
            {
                content = "🚀 **SİSTEM TESTİ:** VarietyShop Karargahı Discord'a başarıyla bağlandı! Börü botları emir bekliyor."
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(webhookUrl, jsonContent);

            if (response.IsSuccessStatusCode)
                return Ok("Test mesajı Discord'a fırlatıldı!");
            else
                return StatusCode((int)response.StatusCode, "Discord'a ulaşılamadı.");
        }

        // URL: POST /tools/webhooks/discord/market-alerts
        [HttpPost("discord/market-alerts")]
        [AllowAnonymous] // İç sistemden (NexusController içinden) de tetiklenebilir
        public async Task<IActionResult> SendMarketAlert([FromBody] string message)
        {
            var webhookUrl = _configuration["DiscordWebhooks:MarketAlerts"];
            if (string.IsNullOrEmpty(webhookUrl)) return Ok(); // Url yoksa hata verme, sadece atla

            var payload = new DiscordPayloadDto { content = $"📢 **YENİ İLAN:** {message}" };
            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(webhookUrl, jsonContent);
            return Ok();
        }

        // URL: POST /tools/webhooks/discord/sales-alerts
        [HttpPost("discord/sales-alerts")]
        [AllowAnonymous]
        public async Task<IActionResult> SendSalesAlert([FromBody] string message)
        {
            var webhookUrl = _configuration["DiscordWebhooks:SalesAlerts"];
            if (string.IsNullOrEmpty(webhookUrl)) return Ok();

            var payload = new DiscordPayloadDto { content = $"💰 **YENİ SATIŞ:** {message}" };
            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync(webhookUrl, jsonContent);
            return Ok();
        }
    }
}