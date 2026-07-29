using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    [Route("announcements")]
    [ApiController]
    public partial class AnnouncementsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnnouncementsController(AppDbContext context)
        {
            _context = context;
        }
    }
}