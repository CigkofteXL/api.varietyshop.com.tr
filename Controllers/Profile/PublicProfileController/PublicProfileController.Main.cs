using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Profile
{
    [Route("profile/public")]
    [ApiController]
    public partial class PublicProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PublicProfileController(AppDbContext context)
        {
            _context = context;
        }
    }
}