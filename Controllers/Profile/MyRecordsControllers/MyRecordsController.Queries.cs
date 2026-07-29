using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.varietyshop.com.tr.Controllers.Profile
{
    public partial class MyRecordsController : ControllerBase
    {
        // URL: GET /profile/records/assets
        [HttpGet("assets")]
        public async Task<IActionResult> GetMyAssets()
        {
            var myId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assets = await _context.UserAssets
                .Where(a => a.UserId == myId)
                .OrderByDescending(a => a.SoldAt)
                .Select(a => new {
                    id = a.Id,
                    productName = a.ProductName,
                    keyData = a.KeyData,
                    soldAt = a.SoldAt
                })
                .ToListAsync();

            return Ok(assets);
        }

        // URL: GET /profile/records/transactions
        [HttpGet("transactions")]
        public async Task<IActionResult> GetMyTransactions()
        {
            var myId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var transactions = await _context.TransactionRecords
                .Where(t => t.UserId == myId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new {
                    id = t.Id,
                    type = t.Type,
                    description = t.Description,
                    amount = t.Amount,
                    createdAt = t.CreatedAt
                })
                .ToListAsync();

            return Ok(transactions);
        }

        
    }
}