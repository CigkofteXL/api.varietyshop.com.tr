using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;
using api.varietyshop.com.tr.Services;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    [Route("market")]
    [ApiController]
    public partial class MarketController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMailService _mailService;

        public MarketController(AppDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }
    }
}