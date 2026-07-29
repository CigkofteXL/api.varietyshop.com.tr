using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using api.varietyshop.com.tr.Core.Entities;
using api.varietyshop.com.tr.DTOs.Economy;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    public partial class EconomyWalletController : ControllerBase
    {
        // URL: POST /economy/wallet/deposit
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return NotFound();

            user.Balance += request.Amount;

            var record = new TransactionRecord
            {
                UserId = userId,
                Amount = request.Amount,
                Type = "Deposit",
                Status = "Completed",
                Description = $"{request.PaymentMethod} üzerinden bakiye yüklendi."
            };

            _context.TransactionRecords.Add(record);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Bakiye başarıyla yüklendi.", NewBalance = user.Balance });
        }

        // URL: POST /economy/wallet/withdraw
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return NotFound();

            if (user.Balance < request.Amount)
            {
                return BadRequest("Yetersiz bakiye.");
            }

            user.Balance -= request.Amount;

            var record = new TransactionRecord
            {
                UserId = userId,
                Amount = -request.Amount,
                Type = "Withdraw",
                Status = "Pending",
                Description = $"Iban'a Çekim Talebi: {request.Iban}"
            };

            _context.TransactionRecords.Add(record);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Para çekme talebiniz alındı, admin onayından sonra iletilecektir.", RemainingBalance = user.Balance });
        }
    }
}