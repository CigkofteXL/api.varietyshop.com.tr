using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    [Route("admin/market")]
    [ApiController]
    [Authorize(Roles = "Admin,Founder")]
    public partial class AdminMarketController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminMarketController(AppDbContext context)
        {
            _context = context;
        }
    }
}