using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    [Route("economy/wallet")]
    [ApiController]
    [Authorize]
    public partial class EconomyWalletController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EconomyWalletController(AppDbContext context)
        {
            _context = context;
        }
    }
}