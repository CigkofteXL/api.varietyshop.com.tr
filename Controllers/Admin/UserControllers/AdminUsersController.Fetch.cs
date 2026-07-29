using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminUsersController : ControllerBase
    {
        // URL: GET /admin/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            var result = allUsers.Select(u => new
            {
                UserId = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                Balance = u.Balance,
                CreatedAt = u.CreatedAt,
            });

            return Ok(result);
        }
    }
}