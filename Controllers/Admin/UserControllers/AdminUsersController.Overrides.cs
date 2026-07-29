using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api.varietyshop.com.tr.Core.Entities;
using api.varietyshop.com.tr.DTOs.Admin;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminUsersController : ControllerBase
    {
        // URL: POST /admin/users/{userId}/balance-override
        [HttpPost("{userId}/balance-override")]
        public async Task<IActionResult> OverrideBalance(Guid userId, [FromBody] BalanceOverrideDto request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("Ajan bulunamadı.");

            var oldBalance = user.Balance;
            user.Balance = request.NewBalance;

            var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var record = new TransactionRecord
            {
                UserId = user.Id,
                Amount = request.NewBalance - oldBalance,
                Type = "Admin Override",
                Status = "Completed",
                Description = $"Admin Müdahalesi: {request.Reason} (Eski: {oldBalance}, Yeni: {request.NewBalance})"
            };

            _context.TransactionRecords.Add(record);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Bakiye başarıyla güncellendi.", user.Balance });
        }
    }
}