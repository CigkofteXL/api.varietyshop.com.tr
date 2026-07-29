using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers
{
    public partial class SettingsController : ControllerBase
    {
        // SADECE ADMİN ŞALTERİ İNDİREBİLİR
        [HttpPost("maintenance/toggle")]
        [Authorize(Roles = "Admin,Founder")]
        public async Task<IActionResult> ToggleMaintenance()
        {
            var settings = await _context.SiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                // Eğer hiç ayar yoksa yeni oluşturup ekliyoruz
                settings = new SiteSetting { Id = 1, IsMaintenanceMode = true };
                _context.SiteSettings.Add(settings);
            }
            else
            {
                // Eğer ayar ZATEN VARSA, sadece değerini tersine çeviriyoruz
                settings.IsMaintenanceMode = !settings.IsMaintenanceMode;
                _context.SiteSettings.Update(settings);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                isMaintenanceMode = settings.IsMaintenanceMode,
                message = settings.IsMaintenanceMode ? "Siber Ağ BAKIMA alındı!" : "Siber Ağ AKTİF edildi!"
            });
        }
    }
}