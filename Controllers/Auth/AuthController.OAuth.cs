using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth;
using System.Net.Http.Headers;
using System.Text.Json;
using api.varietyshop.com.tr.Core.Entities;
using api.varietyshop.com.tr.DTOs.Auth;

namespace api.varietyshop.com.tr.Controllers.Auth
{
    public partial class AuthController : ControllerBase
    {
        public class GoogleLoginDto
        {
            public string IdToken { get; set; }
            public string? ReferralCode { get; set; }
        }

        // =======================================================
        // 🚀 3. GOOGLE İLE GİRİŞ / KAYIT (OAUTH)
        // =======================================================
        [HttpPost("google")]
        public async Task<ActionResult<TokenResponseDto>> GoogleLogin([FromBody] GoogleLoginDto request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

                var email = payload.Email;
                var username = payload.Name.Replace(" ", "").ToLower() + new Random().Next(100, 999);
                var avatarUrl = payload.Picture;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    var baseName = payload.Name.Replace(" ", "").ToLower();
                    var newReferralCode = $"{baseName.Substring(0, Math.Min(4, baseName.Length)).ToUpper()}{new Random().Next(1000, 9999)}";

                    Guid? inviterId = null;
                    if (!string.IsNullOrWhiteSpace(request.ReferralCode))
                    {
                        var inviter = await _context.Users.FirstOrDefaultAsync(u => u.ReferralCode == request.ReferralCode);
                        if (inviter != null) inviterId = inviter.Id;
                    }

                    user = new User
                    {
                        Username = username,
                        Email = email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                        AvatarUrl = avatarUrl,
                        Balance = 0,
                        Role = "User",
                        CreatedAt = DateTime.UtcNow,
                        ReferralCode = newReferralCode,
                        ReferredById = inviterId,
                        IsEmailVerified = true // 🚨 GOOGLE VIP: Doğrulanmış olarak başlasın!
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (user.IsBanned) return StatusCode(403, $"Karargahtan Uzaklaştırıldınız! Sebep: {user.BanReason ?? "Kara Liste"}");

                    if (string.IsNullOrEmpty(user.ReferralCode))
                    {
                        var baseName = user.Username.Length >= 4 ? user.Username.Substring(0, 4) : user.Username;
                        user.ReferralCode = $"{baseName.ToUpper()}{new Random().Next(1000, 9999)}";
                        await _context.SaveChangesAsync();
                    }
                }

                await LogUserLogin(user.Id);

                string token = CreateToken(user);

                return Ok(new TokenResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Role = user.Role,
                    Balance = user.Balance
                });
            }
            catch (Exception)
            {
                return BadRequest("Google siber güvenlik doğrulaması başarısız oldu.");
            }
        }

        public class DiscordLoginDto
        {
            public string AccessToken { get; set; }
            public string? ReferralCode { get; set; }
        }

        // =======================================================
        // 🎮 4. DISCORD İLE GİRİŞ / KAYIT (OAUTH)
        // =======================================================
        [HttpPost("discord")]
        public async Task<ActionResult<TokenResponseDto>> DiscordLogin([FromBody] DiscordLoginDto request)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);

                var response = await httpClient.GetAsync("https://discord.com/api/users/@me");
                if (!response.IsSuccessStatusCode) return BadRequest("Discord doğrulama başarısız.");

                var jsonString = await response.Content.ReadAsStringAsync();
                var discordUser = JsonSerializer.Deserialize<JsonElement>(jsonString);

                var email = discordUser.GetProperty("email").GetString();
                var username = discordUser.GetProperty("username").GetString();
                var discordId = discordUser.GetProperty("id").GetString();

                var avatarHash = discordUser.TryGetProperty("avatar", out var avatarProp) && avatarProp.ValueKind != JsonValueKind.Null
                    ? avatarProp.GetString()
                    : null;
                var avatarUrl = avatarHash != null ? $"https://cdn.discordapp.com/avatars/{discordId}/{avatarHash}.png" : "";

                if (string.IsNullOrEmpty(email)) return BadRequest("Discord hesabınızda e-posta onaylı değil.");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    var baseName = username.Replace(" ", "").ToLower();
                    var newReferralCode = $"{baseName.Substring(0, Math.Min(4, baseName.Length)).ToUpper()}{new Random().Next(1000, 9999)}";

                    Guid? inviterId = null;
                    if (!string.IsNullOrWhiteSpace(request.ReferralCode))
                    {
                        var inviter = await _context.Users.FirstOrDefaultAsync(u => u.ReferralCode == request.ReferralCode);
                        if (inviter != null) inviterId = inviter.Id;
                    }

                    user = new User
                    {
                        Email = email,
                        Username = username + new Random().Next(100, 999),
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                        AvatarUrl = avatarUrl,
                        Balance = 0,
                        Role = "User",
                        CreatedAt = DateTime.UtcNow,
                        ReferralCode = newReferralCode,
                        ReferredById = inviterId,
                        IsEmailVerified = true // 🚨 GOOGLE VIP: Doğrulanmış olarak başlasın!
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (user.IsBanned) return StatusCode(403, $"Karargahtan Uzaklaştırıldınız! Sebep: {user.BanReason ?? "Kara Liste"}");

                    if (string.IsNullOrEmpty(user.ReferralCode))
                    {
                        var baseName = user.Username.Length >= 4 ? user.Username.Substring(0, 4) : user.Username;
                        user.ReferralCode = $"{baseName.ToUpper()}{new Random().Next(1000, 9999)}";
                        await _context.SaveChangesAsync();
                    }
                }

                await LogUserLogin(user.Id);

                string token = CreateToken(user);

                return Ok(new TokenResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Role = user.Role,
                    Balance = user.Balance
                });
            }
            catch (Exception)
            {
                return BadRequest("Discord siber ağına ulaşılamadı.");
            }
        }
    }
}