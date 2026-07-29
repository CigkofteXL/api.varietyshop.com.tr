using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers
{
    public partial class SettingsController : ControllerBase
    {
        // Ziyaretçiler siteye girerken buraya bakar
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSettings()
        {
            var settings = await _context.SiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new SiteSetting { Id = 1 };
                _context.SiteSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            return Ok(settings);
        }
    }
}