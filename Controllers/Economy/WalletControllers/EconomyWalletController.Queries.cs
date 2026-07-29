using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    public partial class EconomyWalletController : ControllerBase
    {
        // URL: GET /economy/wallet/balance
        [HttpGet("balance")]
        public async Task<IActionResult> GetMyBalance()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return NotFound();

            return Ok(new { Balance = user.Balance});
        }

        // URL: GET /economy/wallet/transactions
        [HttpGet("transactions")]
        public async Task<IActionResult> GetMyTransactions()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var history = await _context.TransactionRecords
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new { t.Amount, t.Type, t.Status, t.Description, t.CreatedAt })
                .ToListAsync();

            return Ok(history);
        }
    }
}