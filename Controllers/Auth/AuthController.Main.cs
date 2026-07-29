using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.varietyshop.com.tr.Data;
using api.varietyshop.com.tr.Core.Entities;
using api.varietyshop.com.tr.Services;

namespace api.varietyshop.com.tr.Controllers.Auth
{
    [Route("auth")]
    [ApiController]
    public partial class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AuthController(AppDbContext context, IConfiguration configuration, IMailService mailService)
        {
            _context = context;
            _configuration = configuration;
            _mailService = mailService;
        }

        // =======================================================
        // 🔐 ARKA PLAN 1: JWT TOKEN ÜRETİCİSİ
        // =======================================================
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("ReferralCode", user.ReferralCode ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtSettings:Secret").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JwtSettings:Issuer").Value,
                audience: _configuration.GetSection("JwtSettings:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // =======================================================
        // 🕵️ ARKA PLAN 2: İSTİHBARAT (IP VE CİHAZ) KAYDEDİCİ
        // =======================================================
        private async Task LogUserLogin(Guid userId)
        {
            try
            {
                var ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();

                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                }

                if (ipAddress == "::1" || ipAddress == "127.0.0.1")
                {
                    ipAddress = "127.0.0.1 (Kendi Bilgisayarın / Localhost)";
                }
                else if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = "Sinyal Bulunamadı";
                }

                var deviceInfo = Request.Headers["User-Agent"].ToString();

                var loginLog = new UserLoginLog
                {
                    UserId = userId,
                    IpAddress = ipAddress,
                    DeviceInfo = deviceInfo,
                    LoginDate = DateTime.UtcNow
                };

                _context.UserLoginLogs.Add(loginLog);
                await _context.SaveChangesAsync();
            }
            catch
            {
            }
        }
    }
}