using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    [Route("notifications")]
    [ApiController]
    [Authorize]
    public partial class NotificationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationsController(AppDbContext context)
        {
            _context = context;
        }
    }
}