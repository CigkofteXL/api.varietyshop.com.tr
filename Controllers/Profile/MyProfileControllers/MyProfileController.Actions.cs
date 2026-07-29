using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api.varietyshop.com.tr.DTOs.Profile;

namespace api.varietyshop.com.tr.Controllers.Profile
{
    public partial class MyProfileController : ControllerBase
    {
        // URL: PUT /profile/me/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto request)
        {
            var myId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(myId);

            if (user == null) return NotFound();

            user.Bio = request.Bio;
            user.AvatarUrl = request.AvatarUrl;

            await _context.SaveChangesAsync();
            return Ok("Profiliniz başarıyla güncellendi.");
        }

        // URL: POST /profile/me/security/change-password
        [HttpPost("security/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            var myId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(myId);

            if (user == null) return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                return BadRequest("Eski şifreniz hatalı.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();

            return Ok("Şifreniz başarıyla değiştirildi. Lütfen yeni şifrenizle tekrar giriş yapın.");
        }

        // URL: POST /profile/me/privacy/toggle-visibility
        [HttpPost("privacy/toggle-visibility")]
        public async Task<IActionResult> ToggleVisibility()
        {
            var myId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(myId);

            if (user == null) return NotFound();

            user.IsPublicProfile = !user.IsPublicProfile;
            await _context.SaveChangesAsync();

            var state = user.IsPublicProfile ? "Herkese Açık" : "Gizli";
            return Ok($"Profiliniz artık {state} durumundadır.");
        }
    }
}