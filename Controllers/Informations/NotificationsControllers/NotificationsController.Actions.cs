using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    public partial class NotificationsController : ControllerBase
    {
        // 1. UÇ: TÜM BİLDİRİMLERİ ÇEK (SON 50 SİNYAL)
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    id = n.Id,
                    productId = n.ProductId,
                    type = n.Type,
                    title = n.Title,
                    message = n.Message,
                    createdAt = n.CreatedAt,
                    read = n.IsRead
                })
                .Take(50)
                .ToListAsync();

            return Ok(notifications);
        }

        // 2. UÇ: TÜMÜNÜ OKUNDU İŞARETLE (RADARI TEMİZLE)
        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var unreadNotifs = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            if (!unreadNotifs.Any()) return Ok();

            foreach (var notif in unreadNotifs)
            {
                notif.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Siber ağdaki bildirimler temizlendi." });
        }

        // 3. UÇ: TEKLİ OKUNDU İŞARETLEME METODU
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkSingleAsRead(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var notif = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (notif != null && !notif.IsRead)
            {
                notif.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}