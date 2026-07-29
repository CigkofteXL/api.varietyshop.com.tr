using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers.Support
{
    public partial class TicketController : ControllerBase
    {
        // ========================================================
        // 2. NORMAL KULLANICI UÇLARI (USER)
        // ========================================================

        [HttpGet("tickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var tickets = await _context.Tickets
                .Where(t => t.CreatorId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new { t.Id, t.Subject, t.Status, t.Priority, t.CreatedAt })
                .ToListAsync();

            return Ok(tickets);
        }

        [HttpGet("tickets/{id}")]
        public async Task<IActionResult> GetTicketDetails(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = role == "Admin" || role == "Founder";

            var ticket = await _context.Tickets
                .Include(t => t.Replies)
                .ThenInclude(r => r.Sender)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound("Bilet bulunamadı.");
            if (!isAdmin && ticket.CreatorId != userId) return Unauthorized("Erişim reddedildi.");

            var result = new
            {
                ticket.Id,
                ticket.Subject,
                ticket.Status,
                ticket.CreatedAt,
                Replies = ticket.Replies.OrderBy(r => r.CreatedAt).Select(r => new
                {
                    r.Id,
                    r.Message,
                    IsAdmin = r.IsAdmin,
                    SenderName = r.Sender.Username,
                    r.CreatedAt
                })
            };

            return Ok(result);
        }

        public class CreateTicketDto { public string Subject { get; set; } public string InitialMessage { get; set; } public string Priority { get; set; } = "Normal"; }

        [HttpPost("tickets/create")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var ticket = new Ticket
            {
                CreatorId = userId,
                Subject = request.Subject,
                Status = "Open",
                Priority = request.Priority,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var reply = new TicketReply
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Message = request.InitialMessage,
                IsAdmin = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.TicketReplies.Add(reply);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Destek sinyali merkeze ulaştı." });
        }

        public class ReplyDto { public string Message { get; set; } }

        [HttpPost("tickets/{id}/reply")]
        public async Task<IActionResult> ReplyToTicket(Guid id, [FromBody] ReplyDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = role == "Admin" || role == "Founder";

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();
            if (ticket.Status == "Closed") return BadRequest("Kapalı bilete cevap yazılamaz.");
            if (!isAdmin && ticket.CreatorId != userId) return Unauthorized();

            var reply = new TicketReply
            {
                TicketId = id,
                SenderId = userId,
                Message = request.Message,
                IsAdmin = isAdmin,
                CreatedAt = DateTime.UtcNow
            };

            ticket.Status = isAdmin ? "Answered" : "Open";

            _context.TicketReplies.Add(reply);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("tickets/{id}/close")]
        public async Task<IActionResult> CloseTicket(Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isAdmin = role == "Admin" || role == "Founder";

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();
            if (!isAdmin && ticket.CreatorId != userId) return Unauthorized();

            ticket.Status = "Closed";
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}