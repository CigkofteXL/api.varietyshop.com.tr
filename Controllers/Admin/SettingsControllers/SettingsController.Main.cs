using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers
{
    [Route("settings")]
    [ApiController]
    public partial class SettingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }
    }
}