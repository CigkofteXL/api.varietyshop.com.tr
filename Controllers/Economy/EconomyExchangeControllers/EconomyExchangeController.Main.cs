using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    [Route("economy/exchange")]
    [ApiController]
    [Authorize]
    public partial class EconomyExchangeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EconomyExchangeController(AppDbContext context)
        {
            _context = context;
        }
    }
}