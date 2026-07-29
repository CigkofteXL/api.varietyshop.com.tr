using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Core.Entities;
using api.varietyshop.com.tr.DTOs.Admin;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminSystemController : ControllerBase
    {
        // DUYURU OLUŞTUR
        [HttpPost("announcements")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto request)
        {
            var announcement = new Announcement { Title = request.Title, Content = request.Content, Type = request.Type, CreatedAt = DateTime.UtcNow };
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Siber ağa yeni duyuru başarıyla fırlatıldı!", announcement });
        }

        // DUYURU SİL
        [HttpDelete("announcements/{id}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound("Silinecek duyuru bulunamadı.");
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Duyuru sistemden tamamen kazındı." });
        }
    }
}