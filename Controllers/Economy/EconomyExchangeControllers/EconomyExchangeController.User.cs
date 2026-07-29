using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    public partial class EconomyExchangeController : ControllerBase
    {
        public class CreateExchangeDto
        {
            public string CodeOrItem { get; set; } // Gönderilen E-pin kodu veya hesap bilgisi
            public string Platform { get; set; }   // Örn: Wolvesville, Valorant, Steam
            public decimal EstimatedValue { get; set; } // Kullanıcının beklediği tahmini tutar
        }

        // URL: POST /economy/exchange/request
        [HttpPost("request")]
        public async Task<IActionResult> SubmitExchangeRequest([FromBody] CreateExchangeDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (string.IsNullOrWhiteSpace(request.CodeOrItem))
                return BadRequest("Takas edilecek kod veya içerik boş olamaz.");

            // Yeni bir takas talebi oluştur (Başlangıç durumu: Pending)
            var exchangeRequest = new ExchangeRequest
            {
                UserId = userId,
                CodeOrItem = request.CodeOrItem,
                Platform = request.Platform,
                EstimatedValue = request.EstimatedValue,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.ExchangeRequests.Add(exchangeRequest);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Takas talebiniz güvenli havuza iletildi. Admin kontrolünden sonra bakiyeniz yüklenecektir." });
        }

        // URL: GET /economy/exchange/my-requests
        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyExchangeRequests()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var history = await _context.ExchangeRequests
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new {
                    e.Id,
                    e.Platform,
                    e.EstimatedValue,
                    e.Status,
                    e.CreatedAt
                })
                .ToListAsync();

            return Ok(history);
        }
    }
}