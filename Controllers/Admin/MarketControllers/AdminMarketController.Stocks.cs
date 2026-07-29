using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminMarketController : ControllerBase
    {
        // 5. ÜRÜNE YENİ LİSANS (STOK) EKLE
        [HttpPost("product/stock")]
        public async Task<IActionResult> AddStock([FromBody] List<string> keys, [FromQuery] Guid productId)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
            if (!productExists) return NotFound("Böyle bir ürün yok.");

            var newLicenses = keys.Select(key => new LicenseKey
            {
                ProductId = productId,
                KeyData = key,
                IsSold = false
            }).ToList();

            _context.LicenseKeys.AddRange(newLicenses);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"{keys.Count} adet yeni stok sisteme enjekte edildi." });
        }

        // 6. ÜRÜNÜN İÇİNDEKİ MEVCUT ŞİFRELERİ GÖR
        [HttpGet("product/{id}/stock")]
        public async Task<IActionResult> GetProductStock(Guid id)
        {
            var keys = await _context.LicenseKeys
                .Where(l => l.ProductId == id && !l.IsSold)
                .Select(l => new { l.Id, l.KeyData })
                .ToListAsync();

            return Ok(keys);
        }

        // 7. HATALI EKLENEN TEKLİ ŞİFREYİ SİL
        [HttpDelete("stock/{keyId}")]
        public async Task<IActionResult> DeleteSingleStock(Guid keyId)
        {
            var key = await _context.LicenseKeys.FindAsync(keyId);
            if (key == null) return NotFound("Lisans anahtarı bulunamadı.");

            if (key.IsSold) return BadRequest("Satılmış bir lisansı silemezsiniz!");

            _context.LicenseKeys.Remove(key);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Hatalı lisans sistemden silindi." });
        }
    }
}