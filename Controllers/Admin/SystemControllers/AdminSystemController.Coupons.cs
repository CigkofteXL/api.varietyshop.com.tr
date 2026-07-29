using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminSystemController : ControllerBase
    {
        // TÜM KUPONLARI VE KULLANIM DETAYLARINI GETİR
        [HttpGet("coupons")]
        public async Task<IActionResult> GetAllCoupons()
        {
            var coupons = await _context.Coupons
                .Include(c => c.Usages).ThenInclude(u => u.User)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new {
                    id = c.Id,
                    code = c.Code,
                    rewardAmount = c.RewardAmount,
                    maxUsage = c.MaxUsage,
                    currentUsage = c.CurrentUsage,
                    isActive = c.IsActive,
                    createdAt = c.CreatedAt,
                    usedBy = c.Usages.Select(u => new { userId = u.UserId, username = u.User.Username, usedAt = u.UsedAt }).ToList()
                })
                .ToListAsync();
            return Ok(coupons);
        }
    }
}