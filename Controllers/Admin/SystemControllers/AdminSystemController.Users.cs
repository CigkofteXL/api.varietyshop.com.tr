using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminSystemController : ControllerBase
    {
        // AJAN LİSTESİNİ ÇEK
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Balance,
                    u.Role,
                    u.IsBanned,
                    u.BanReason,
                    LastLoginIp = _context.UserLoginLogs
                        .Where(l => l.UserId == u.Id)
                        .OrderByDescending(l => l.LoginDate)
                        .Select(l => l.IpAddress)
                        .FirstOrDefault()
                })
                .ToListAsync();
            return Ok(users);
        }

        // AJAN BANLA / BAN KALDIR
        [HttpPost("users/{id}/ban")]
        public async Task<IActionResult> ToggleUserBan(Guid id, [FromBody] string? reason)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("Ajan bulunamadı.");

            user.IsBanned = !user.IsBanned;
            user.BanReason = user.IsBanned ? (reason ?? "Belirtilmemiş") : null;

            await _context.SaveChangesAsync();
            return Ok(new { message = user.IsBanned ? "Ajan kara listeye alındı." : "Ajanın banı kaldırıldı." });
        }

        // AJAN GİRİŞ LOGLARINI GETİR
        [HttpGet("users/{id}/logs")]
        public async Task<IActionResult> GetUserLoginLogs(Guid id)
        {
            var logs = await _context.UserLoginLogs
                .Where(l => l.UserId == id)
                .OrderByDescending(l => l.LoginDate)
                .Take(50)
                .ToListAsync();
            return Ok(logs);
        }
    }
}