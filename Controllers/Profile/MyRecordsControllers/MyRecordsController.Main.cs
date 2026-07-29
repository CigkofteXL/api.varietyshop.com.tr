using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Profile
{
    [Route("profile/records")]
    [ApiController]
    [Authorize]
    public partial class MyRecordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MyRecordsController(AppDbContext context)
        {
            _context = context;
        }
    }
}