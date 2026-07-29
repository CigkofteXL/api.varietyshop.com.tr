using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    public partial class EconomyCouponsController : ControllerBase
    {
        // URL: POST /economy/coupons/redeem
        [HttpPost("redeem")]
        [Authorize]
        public async Task<IActionResult> RedeemCoupon([FromBody] string code)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            var coupon = await _context.Coupons
                .Include(c => c.Usages)
                .FirstOrDefaultAsync(c => c.Code == code.ToUpper() && c.IsActive);

            if (coupon == null || coupon.CurrentUsage >= coupon.MaxUsage)
                return BadRequest("Bu kupon geçersiz veya kullanım sınırı dolmuş.");

            if (coupon.Usages.Any(u => u.UserId == userId))
                return BadRequest("Bu promosyon kodunu daha önce kullandınız!");

            user!.Balance += coupon.RewardAmount;
            coupon.CurrentUsage += 1;

            if (coupon.CurrentUsage >= coupon.MaxUsage)
                coupon.IsActive = false;

            var usageRecord = new CouponUsage
            {
                CouponId = coupon.Id,
                UserId = userId,
                UsedAt = DateTime.UtcNow
            };
            _context.CouponUsages.Add(usageRecord);

            var transaction = new TransactionRecord
            {
                UserId = userId,
                Amount = coupon.RewardAmount,
                Type = "CouponRedeem",
                Status = "Completed",
                Description = $"Promosyon Kodu Kullanıldı: {coupon.Code}"
            };
            _context.TransactionRecords.Add(transaction);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Kupon başarıyla bozduruldu!",
                AddedAmount = coupon.RewardAmount,
                NewBalance = user.Balance
            });
        }
    }
}