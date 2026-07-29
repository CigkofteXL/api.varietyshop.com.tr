using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Profile
{
    public partial class PublicProfileController : ControllerBase
    {
        // URL: GET /profile/public/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProfileById(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound("Ajan bulunamadı.");
            if (!user.IsPublicProfile) return BadRequest("Bu ajanın profili gizlidir.");

            return Ok(new
            {
                user.Username,
                user.Bio,
                user.AvatarUrl,
                user.CreatedAt,
                
            });
        }

        // URL: GET /profile/public/nick/{nick}
        [HttpGet("nick/{nick}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProfileByNick(string nick)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == nick.ToLower());

            if (user == null) return NotFound("Ajan bulunamadı.");
            if (!user.IsPublicProfile) return BadRequest("Bu ajanın profili gizlidir.");

            return Ok(new
            {
                user.Username,
                user.Bio,
                user.AvatarUrl,
                user.CreatedAt,
               
            });
        }
    }
}