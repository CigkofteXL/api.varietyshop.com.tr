using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.varietyshop.com.tr.Controllers.Auth
{
    public partial class AuthController : ControllerBase
    {
        public class ForgotPasswordDto { public string Email { get; set; } }

        // =======================================================
        // 6. ŞİFREMİ UNUTTUM (FORGOT PASSWORD) - MAİL FIRLATICI
        // =======================================================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return Ok("Eğer bu e-posta siber ağda kayıtlıysa, sıfırlama linki gönderilmiştir.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value!));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("Purpose", "ResetPassword")
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature),
                Issuer = _configuration.GetSection("JwtSettings:Issuer").Value,
                Audience = _configuration.GetSection("JwtSettings:Audience").Value
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var resetToken = tokenHandler.WriteToken(token);

            string resetLink = $"http://localhost:5173/reset-password?token={resetToken}";

            string emailBody = $@"
                <div style='background-color:#0a0c12; color:#e4e1e9; padding:30px; border-radius:15px; font-family:sans-serif; border: 1px solid rgba(255,255,255,0.1); max-width: 500px; margin: auto;'>
                    <h2 style='color:#ffc300; text-transform:uppercase; letter-spacing: 2px; border-bottom: 1px solid rgba(255,195,0,0.3); padding-bottom: 10px;'>Şifre Sıfırlama Talebi</h2>
                    <p style='font-size: 16px;'>Komutan <strong>{user.Username}</strong>,</p>
                    <p style='color:#a0a0a0;'>Siber karargahtaki şifreni unuttuğunu bildirdin. Aşağıdaki butona tıklayarak yeni şifreni belirleyebilirsin. Bu yetki kartı siber güvenlik sebebiyle <strong>15 dakika</strong> sonra imha edilecektir.</p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' style='background-color:#ffc300; color:#000; padding:15px 30px; text-decoration:none; font-weight:bold; border-radius:10px; font-size: 14px; text-transform: uppercase; letter-spacing: 1px;'>Şifremi Sıfırla</a>
                    </div>

                    <p style='color:#fb2e00; font-size:11px; font-weight:bold;'>Eğer bu talebi sen yapmadıysan bu mesajı görmezden gel, hesabın güvende.</p>
                    <p style='font-size:10px; color:#555; text-align:center; margin-top:30px;'>VarietyShop Siber Güvenlik Protokolü</p>
                </div>";

            await _mailService.SendEmailAsync(user.Email, "🔑 VarietyShop Siber Ağ Şifre Sıfırlama", emailBody);

            return Ok("Eğer bu e-posta siber ağda kayıtlıysa, sıfırlama linki gönderilmiştir.");
        }

        public class ResetPasswordDto { public string Token { get; set; } public string NewPassword { get; set; } }

        // =======================================================
        // 7. YENİ ŞİFREYİ KAYDETME (RESET PASSWORD)
        // =======================================================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value!);

                tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration.GetSection("JwtSettings:Issuer").Value,
                    ValidateAudience = true,
                    ValidAudience = _configuration.GetSection("JwtSettings:Audience").Value,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var purpose = jwtToken.Claims.First(x => x.Type == "Purpose").Value;
                if (purpose != "ResetPassword") return BadRequest("Geçersiz siber bilet.");

                var userIdStr = jwtToken.Claims.First(x => x.Type == "nameid").Value;
                var user = await _context.Users.FindAsync(Guid.Parse(userIdStr));

                if (user == null) return NotFound("Ajan bulunamadı.");

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                await _context.SaveChangesAsync();

                return Ok("Şifre başarıyla güncellendi! Karargaha yeni şifrenle giriş yapabilirsin.");
            }
            catch (Exception)
            {
                return BadRequest("Sıfırlama linki geçersiz veya süresi dolmuş. Lütfen yeni bir talep oluşturun.");
            }
        }
        // AuthController.Reset.cs dosyasının en altına eklenecek DTO ve Endpoint

        public class VerifyEmailDto
        {
            public string Email { get; set; } = string.Empty;
            public string Token { get; set; } = string.Empty;
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return NotFound("Ajan bulunamadı.");

            if (user.IsEmailVerified)
                return BadRequest("Bu hesap zaten daha önceden onaylanmış kanka.");

            if (user.EmailVerificationToken != request.Token)
                return BadRequest("Girdiğiniz siber doğrulama kodu geçersiz veya sahte!");

            // Kilidi aç ve kodu imha et
            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;

            // Giriş logunu tetikle
            await LogUserLogin(user.Id);
            await _context.SaveChangesAsync();

            // 🚨 OTOMATİK GİRİŞ SİNYALİ: Tıpkı login gibi token üretiyoruz kanka
            string token = CreateToken(user);

            // Verileri paketleyip React'a fırlatıyoruz
            return Ok(new
            {
                Message = "Kilit açıldı! Siber ağa otomatik bağlanılıyorsunuz...",
                Token = token,
                Username = user.Username,
                Role = user.Role,
                Balance = user.Balance
            });
        }
    }
}