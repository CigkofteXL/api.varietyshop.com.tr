using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Profile
{
    [Route("profile/me")]
    [ApiController]
    [Authorize]
    public partial class MyProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MyProfileController(AppDbContext context)
        {
            _context = context;
        }
    }
}