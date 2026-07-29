using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminSystemController : ControllerBase
    {
        // DASHBOARD İSTATİSTİKLERİNİ GETİR
        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalWebUsers = await _context.Users.CountAsync();
            var totalVcInMarket = await _context.Users.SumAsync(u => u.Balance);

            var last5Minutes = DateTime.UtcNow.AddMinutes(-5);
            var activeUsersToday = await _context.UserLoginLogs
                .Where(l => l.LoginDate >= last5Minutes)
                .Select(l => l.UserId)
                .Distinct()
                .CountAsync();

            var totalStock = await _context.LicenseKeys.CountAsync(l => !l.IsSold);
            var openTickets = await _context.Tickets.CountAsync(t => t.Status != "Closed" && t.Status != "Resolved");

            var lastWeek = DateTime.UtcNow.AddDays(-7);
            var rawSales = await _context.TransactionRecords
                .Where(t => t.Type == "Market Purchase" && t.CreatedAt >= lastWeek)
                .Select(t => new { t.CreatedAt, t.Amount })
                .ToListAsync();

            var weeklySales = rawSales
                .GroupBy(t => t.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new {
                    Date = g.Key.ToString("dd.MM"),
                    TotalSales = g.Count(),
                    TotalRevenue = g.Sum(x => Math.Abs(x.Amount))
                })
                .ToList();

            return Ok(new
            {
                TotalWebUsers = totalWebUsers,
                TotalEconomyVc = totalVcInMarket,
                ActiveUsersToday = activeUsersToday,
                TotalStock = totalStock,
                OpenTickets = openTickets,
                WeeklySales = weeklySales
            });
        }

        // DENETİM GÜNLÜKLERİNİ ÇEK
        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetAuditLogs()
        {
            var logs = await _context.TransactionRecords
                .OrderByDescending(t => t.CreatedAt)
                .Take(100)
                .Select(t => new { t.Id, Date = t.CreatedAt, User = t.User.Username, Action = t.Type, Target = t.Description, Level = t.Amount > 1000 ? "warning" : "info" })
                .ToListAsync();
            return Ok(logs);
        }
    }
}