using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.varietyshop.com.tr.Core.Entities;
using api.varietyshop.com.tr.DTOs.Economy;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    public partial class EconomyCouponsController : ControllerBase
    {
        // URL: POST /economy/coupons/generate
        [HttpPost("generate")]
        [Authorize(Roles = "Admin,Founder")]
        public async Task<IActionResult> GenerateCoupon([FromBody] GenerateCouponDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                return BadRequest("Kupon kodu boş olamaz.");

            if (await _context.Coupons.AnyAsync(c => c.Code == request.Code.ToUpper()))
                return BadRequest("Bu kupon kodu sistemde zaten mevcut!");

            var coupon = new Coupon
            {
                Code = request.Code.ToUpper(),
                RewardAmount = request.Amount,
                MaxUsage = request.MaxUsage,
                CurrentUsage = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Kupon Üretildi: {coupon.Code} | Değer: {coupon.RewardAmount} | Kullanım Hakkı: {coupon.MaxUsage}" });
        }
    }
}