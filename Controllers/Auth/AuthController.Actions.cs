using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using api.varietyshop.com.tr.Core.Entities;
using api.varietyshop.com.tr.DTOs.Auth;

namespace api.varietyshop.com.tr.Controllers.Auth
{
    public partial class AuthController : ControllerBase
    {
        // =======================================================
        // 1. NORMAL KAYIT OLMA (REGISTER)
        // =======================================================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email || u.Username == request.Username))
            {
                return BadRequest("Bu kullanıcı adı veya e-posta zaten kullanımda.");
            }

            var randomNum = new Random().Next(1000, 9999);
            var baseName = request.Username.Length >= 4 ? request.Username.Substring(0, 4) : request.Username;
            var newReferralCode = $"{baseName.ToUpper()}{randomNum}";

            Guid? inviterId = null;
            if (!string.IsNullOrWhiteSpace(request.ReferralCode))
            {
                var inviter = await _context.Users.FirstOrDefaultAsync(u => u.ReferralCode == request.ReferralCode);
                if (inviter != null)
                {
                    inviterId = inviter.Id;
                }
            }
            // 6 haneli güvenli doğrulama kodu üretiyoruz
            var verificationToken = new Random().Next(100000, 999999).ToString();

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Balance = 0,
                ReferralCode = newReferralCode,
                ReferredById = inviterId,
                CreatedAt = DateTime.UtcNow,
                IsEmailVerified = false, // 🚨 İlk kayıt anında kapı kilitli!
                EmailVerificationToken = verificationToken // Kod buraya mühürlendi
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // ====================================================================
            // ✉️ KULLANICIYA SİBER ONAY MAİLİ FIRLATMA
            // ====================================================================
            try
            {
                string subject = "🔑 VarietyShop Siber Ağ Aktivasyon Kodu";
                string emailBody = $@"
        <div style='background-color:#0a0c12; color:#e4e1e9; padding:30px; border-radius:15px; font-family:sans-serif; border: 1px solid rgba(255,255,255,0.1); max-width: 500px; margin: auto;'>
            <h2 style='color:#0af2ff; text-transform:uppercase; letter-spacing: 2px; border-bottom: 1px solid rgba(10,242,255,0.3); padding-bottom: 10px;'>Hesap Doğrulama</h2>
            <p style='font-size: 16px;'>Ajan <strong>{newUser.Username}</strong>,</p>
            <p style='color:#a0a0a0;'>VarietyShop ağına sızabilmek için e-posta adresini doğrulaman gerekiyor. Giriş şifren aşağıda siber olarak mühürlenmiştir:</p>
            <div style='background-color: rgba(0,0,0,0.5); padding: 20px; border-radius: 10px; margin: 20px 0; text-align: center; border: 1px solid #0af2ff;'>
                <span style='font-size: 32px; font-weight: bold; letter-spacing: 5px; color: #0af2ff; font-family: monospace;'>{verificationToken}</span>
            </div>
            <p style='color:#fb2e00; font-size:11px; font-weight:bold;'>Eğer bu işlemi sen yapmadıysan bu kodu kimseyle paylaşma.</p>
            <p style='font-size:10px; color:#555; text-align:center; margin-top:30px;'>Varietyshop Siber Otomasyon Sistemleri</p>
        </div>";

                await _mailService.SendEmailAsync(newUser.Email, subject, emailBody);
            }
            catch
            { }


            return Ok("Ajan kaydı başarıyla tamamlandı.");
        }

        // =======================================================
        // 2. NORMAL GİRİŞ YAPMA (LOGIN) VE TEST MAİLİ ATMA 🚀
        // =======================================================
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Geçersiz e-posta veya şifre.");
            }

            if (user.IsBanned)
                return StatusCode(403, $"Karargahtan Uzaklaştırıldınız! Sebep: {user.BanReason ?? "Kara Liste"}");

            // 🚨 YENİ E-POSTA DUVARI: Hesap onaylı değilse JWT token üretme, kilit fırlat!
            if (!user.IsEmailVerified && user.Role != "Admin")
            {
                return StatusCode(403, new
                {
                    Message = "E-posta adresi doğrulanmamış kanka! Karargaha erişemezsin.",
                    RequiresVerification = true,
                    Email = user.Email
                });
            }

            if (string.IsNullOrEmpty(user.ReferralCode))
            {
                var randomNum = new Random().Next(1000, 9999);
                var baseName = user.Username.Length >= 4 ? user.Username.Substring(0, 4) : user.Username;
                user.ReferralCode = $"{baseName.ToUpper()}{randomNum}";
                await _context.SaveChangesAsync();
            }

            await LogUserLogin(user.Id);

            try
            {
                string subject = "🛡️ Siber Karargaha Yeni Giriş Tespiti";
                string fiyakaliIcerik = $@"
                    <div style='background-color:#0a0c12; color:#e4e1e9; padding:30px; border-radius:15px; font-family:sans-serif; border: 1px solid rgba(255,255,255,0.1); max-width: 500px; margin: auto;'>
                        <h2 style='color:#0af2ff; text-transform:uppercase; letter-spacing: 2px; border-bottom: 1px solid rgba(10,242,255,0.3); padding-bottom: 10px;'>Ağa Bağlanıldı</h2>
                        <p style='font-size: 16px;'>Komutan <strong>{user.Username}</strong>,</p>
                        <p style='color:#a0a0a0;'>Hesabınıza az önce başarılı bir giriş yapıldı. Siber kalkanlar devrede.</p>
                        <div style='background-color: rgba(0,0,0,0.5); padding: 15px; border-radius: 10px; margin: 20px 0; border-left: 3px solid #ffc300;'>
                            <p style='margin:0; font-size:12px; color:#888;'><strong>Tarih:</strong> {DateTime.UtcNow.AddHours(3).ToString("dd.MM.yyyy HH:mm:ss")}</p>
                            <p style='margin:5px 0 0 0; font-size:12px; color:#888;'><strong>Durum:</strong> Başarılı Erişim</p>
                        </div>
                        <p style='color:#fb2e00; font-size:11px; font-weight:bold;'>Eğer bu işlemi sen yapmadıysan, derhal sistem yöneticisiyle iletişime geç.</p>
                        <p style='font-size:10px; color:#555; text-align:center; margin-top:30px;'>Varietyshop Siber Otomasyon Sistemleri</p>
                    </div>";

                await _mailService.SendEmailAsync(user.Email, subject, fiyakaliIcerik);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MAİL MOTORU HATASI]: {ex.Message}");
            }

            string token = CreateToken(user);

            return Ok(new TokenResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                Balance = user.Balance
            });
        }

        // =======================================================
        // 5. TOKEN YENİLEME (REFRESH)
        // =======================================================
        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized("Geçersiz oturum.");

            var user = await _context.Users.FindAsync(Guid.Parse(userIdStr));

            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var newToken = CreateToken(user);

            return Ok(new
            {
                Message = "Token başarıyla yenilendi.",
                token = newToken
            });
        }

        // =======================================================
        // 🚀 PING KONTROLÜ
        // =======================================================
        [HttpPost("ping")]
        [Authorize]
        public async Task<IActionResult> PingUser()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var log = new UserLoginLog
            {
                UserId = userId,
                IpAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Sinyal",
                DeviceInfo = Request.Headers["User-Agent"].ToString(),
                LoginDate = DateTime.UtcNow
            };

            _context.UserLoginLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // =======================================================
        // 9. REFERANS İSTİHBARATI (KAÇ KİŞİ DAVET ETTİ VE LİSTE)
        // =======================================================
        [HttpGet("referral-stats")]
        [Authorize]
        public async Task<IActionResult> GetReferralStats()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var invitedUsers = await _context.Users
                .Where(u => u.ReferredById == userId)
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new {
                    MaskedName = u.Username.Length > 3 ? u.Username.Substring(0, 3) + "***" : u.Username + "***",
                    JoinDate = u.CreatedAt.ToString("dd.MM.yyyy")
                })
                .ToListAsync();

            return Ok(new
            {
                ReferralCode = user.ReferralCode,
                InvitedCount = invitedUsers.Count,
                InvitedList = invitedUsers,
                TotalEarned = 0
            });
        }
    }
}