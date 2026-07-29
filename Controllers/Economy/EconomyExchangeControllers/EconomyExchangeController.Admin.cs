using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    public partial class EconomyExchangeController : ControllerBase
    {
        public class ResolveExchangeDto
        {
            public string Action { get; set; } // "Approve" veya "Reject"
            public decimal FinalAmount { get; set; } // Adminin bizzat belirlediği net bakiye değeri
            public string? RejectionReason { get; set; } // Reddedildiyse sebebi
        }

        // URL: GET /admin/economy/exchanges/pending
        [HttpGet("~/admin/economy/exchanges/pending")] // Admin paneli için rotayı ezdik kanka
        [Authorize(Roles = "Admin,Founder")]
        public async Task<IActionResult> GetPendingExchangesAdmin()
        {
            var pendingRequests = await _context.ExchangeRequests
                .Include(e => e.User)
                .Where(e => e.Status == "Pending")
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new {
                    e.Id,
                    BuyerName = e.User.Username,
                    e.Platform,
                    e.CodeOrItem,
                    e.EstimatedValue,
                    e.CreatedAt
                })
                .ToListAsync();

            return Ok(pendingRequests);
        }

        // URL: POST /economy/exchange/{id}/resolve
        [HttpPost("{id}/resolve")]
        [Authorize(Roles = "Admin,Founder")]
        public async Task<IActionResult> ResolveExchangeRequest(Guid id, [FromBody] ResolveExchangeDto request)
        {
            var exchange = await _context.ExchangeRequests.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);

            if (exchange == null) return NotFound("Takas talebi bulunamadı.");
            if (exchange.Status != "Pending") return BadRequest("Bu talep zaten karara bağlanmış.");

            // 1. DURUM: E-pin Geçerliyse ve Onaylıyorsan
            if (request.Action == "Approve")
            {
                exchange.User.Balance += request.FinalAmount;
                exchange.Status = "Approved";

                // Finansal hareket dökümünü mühürle
                _context.TransactionRecords.Add(new TransactionRecord
                {
                    UserId = exchange.UserId,
                    Amount = request.FinalAmount,
                    Type = "Exchange",
                    Status = "Completed",
                    Description = $"E-Pin / Varlık Takas Geliri ({exchange.Platform})"
                });

                // Ajanın bildirim paneline siber sinyal fırlat
                _context.Notifications.Add(new Notification
                {
                    UserId = exchange.UserId,
                    Title = "⚡ Takas İşleminiz Onaylandı!",
                    Message = $"Göndermiş olduğunuz {exchange.Platform} kodu siber darphanede doğrulanmıştır. {request.FinalAmount} ₺ bakiyenize eklenmiştir.",
                    Type = "success",
                    CreatedAt = DateTime.UtcNow
                });
            }
            // 2. DURUM: Kod Kullanılmış veya Sahteyse (Reddetme)
            else if (request.Action == "Reject")
            {
                exchange.Status = "Rejected";

                _context.Notifications.Add(new Notification
                {
                    UserId = exchange.UserId,
                    Title = "❌ Takas Talebi Reddedildi",
                    Message = $"{exchange.Platform} takas talebiniz admin tarafından reddedildi. Sebep: {request.RejectionReason ?? "Geçersiz veya kullanılmış kod."}",
                    Type = "error",
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                return BadRequest("Geçersiz aksiyon parametresi. Sadece 'Approve' veya 'Reject' geçerlidir.");
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = $"Takas talebi başarıyla sonuçlandırıldı. Karar: {request.Action}" });
        }
    }
}