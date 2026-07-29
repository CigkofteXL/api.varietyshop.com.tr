using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    [Route("admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin,Founder")]
    public partial class AdminUsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminUsersController(AppDbContext context)
        {
            _context = context;
        }
    }
}