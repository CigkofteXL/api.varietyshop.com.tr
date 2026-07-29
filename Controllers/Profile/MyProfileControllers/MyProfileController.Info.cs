using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.varietyshop.com.tr.Controllers.Profile
{
    public partial class MyProfileController : ControllerBase
    {
        // URL: GET /profile/me/info
        [HttpGet("info")]
        public async Task<IActionResult> GetMyProfile()
        {
            var myId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == myId);

            if (user == null) return NotFound();

            return Ok(new
            {
                user.Username,
                user.Email,
                user.Bio,
                user.AvatarUrl,
                user.Balance,
                user.IsPublicProfile,
                user.CreatedAt,
               
            });
        }

        // URL: GET /profile/me/data-export
        [HttpGet("data-export")]
        public async Task<IActionResult> ExportMyData()
        {
            var myId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == myId);

            if (user == null) return NotFound();

            var exportData = new
            {
                PersonalInfo = new { user.Username, user.Email, user.CreatedAt, user.LastKnownIp },
                Economy = new { user.Balance },
               
            };

            return Ok(exportData);
        }
    }
}