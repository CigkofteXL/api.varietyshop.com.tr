using Microsoft.AspNetCore.Mvc;
using api.varietyshop.com.tr.Data;

namespace api.varietyshop.com.tr.Controllers.Economy
{
    [Route("economy/coupons")]
    [ApiController]
    public partial class EconomyCouponsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EconomyCouponsController(AppDbContext context)
        {
            _context = context;
        }
    }
}