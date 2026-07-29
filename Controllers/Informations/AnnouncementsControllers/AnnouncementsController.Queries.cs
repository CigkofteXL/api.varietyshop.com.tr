using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    public partial class AnnouncementsController : ControllerBase
    {
        // URL: GET /announcements
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAnnouncements()
        {
            var announcements = await _context.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new {
                    id = a.Id,
                    title = a.Title,
                    content = a.Content,
                    type = a.Type,
                    createdAt = a.CreatedAt
                })
                .ToListAsync();

            return Ok(announcements);
        }
    }
}