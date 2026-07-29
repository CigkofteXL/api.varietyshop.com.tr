using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Support
{
    public partial class TicketController : ControllerBase
    {
        // ========================================================
        // 🚨 YÖNETİCİ (ADMIN) UCU - REACT'TEKİ 404 HATASINI ÇÖZEN KISIM
        // ========================================================
        [HttpGet("~/admin/support/tickets")] // Başına ~ koyduk ki üstteki 'support' rotasını ezip buraya özel url açsın
        [Authorize(Roles = "Admin,Founder")] // Sadece yetkililer
        public async Task<IActionResult> GetAllTicketsAdmin()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Creator)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new {
                    t.Id,
                    t.Subject,
                    t.Status,
                    t.Priority,
                    t.CreatedAt,
                    CreatorName = t.Creator.Username
                })
                .ToListAsync();

            return Ok(tickets);
        }
    }
}