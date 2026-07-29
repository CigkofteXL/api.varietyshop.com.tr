using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    [Route("admin/system")]
    [ApiController]
    [Authorize(Roles = "Admin,Founder")]
    public partial class AdminSystemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminSystemController(AppDbContext context)
        {
            _context = context;
        }
    }
}