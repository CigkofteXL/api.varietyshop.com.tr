using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using api.varietyshop.com.tr.Data;
using api.varietyshop.com.tr.Core.Entities;
using System.Threading.Tasks;

namespace api.varietyshop.com.tr.Controllers.SaaS
{
    [Authorize]
    [ApiController]
    [Route("saas/billing")] // 🚨 ROTA: Gelecekteki tüm helal SaaS'ların ortak cüzdan/satın alma merkezidir kanka
    public class SaaSBillingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SaaSBillingController(AppDbContext context)
        {
            _context = context;
        }

        // ========================================================
        // 🪙 VARIETYSHOP BAKİYESİ İLE SAAS PAKET SATIN ALMA MOTORU
        // ========================================================
        [HttpPost("purchase-quota")]
        public async Task<IActionResult> PurchaseQuota([FromBody] GeneralSaaSPurchaseRequest model)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("Kullanıcı verisi siber ağda bulunamadı.");

            decimal totalCost = 0;
            string itemDescription = "";

            // 🚨 HELAL SAAS FİYATLANDIRMA MATRİSİ (Ön yüzden gelen paket türüne göre tetiklenir kanka)
            if (model.PackageType == "DISCORD_BOT_PREMIUM") { totalCost = 60m; itemDescription = "Kiralık Bulut Discord Botu Premium Lisansı"; }
            else if (model.PackageType == "CYBER_TREE_PRO") { totalCost = 40m; itemDescription = "Siber Link-in-Bio Tasarımcısı Pro Modülü"; }
            else if (model.PackageType == "OBS_OVERLAY_WIDGET") { totalCost = 50m; itemDescription = "OBS Canlı Yayın Widget & Sayaç Motoru"; }
            else if (model.PackageType == "LIVE_BANNER_GIF") { totalCost = 35m; itemDescription = "7/24 Canlı Profil Banner Motoru"; }
            else return BadRequest("Geçersiz veya tanımsız bir SaaS paket seçimi yapıldı komutanım.");

            // Bakiye kontrolü kanka
            if (user.Balance < totalCost)
                return BadRequest("Yetersiz bakiye. Lütfen karargaha bakiye takviyesi yapın kanka.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Kullanıcı bakiyesinden net tutarı düşüyoruz
                user.Balance -= totalCost;

                // 2. Mevcut TransactionRecords tablosuna finansal izi kazıyoruz kanka
                _context.TransactionRecords.Add(new TransactionRecord
                {
                    UserId = userId,
                    Amount = -totalCost,
                    Type = "SaaS Package Purchase",
                    Status = "Completed",
                    Description = $"{itemDescription} başarıyla VarietyShop bakiyesi ile satın alındı."
                });

                // 3. Kullanıcının paneline canlı başarı bildirimi fırlatıyoruz
                _context.Notifications.Add(new Notification
                {
                    UserId = userId,
                    Title = "🎉 SaaS Lisansı Tanımlandı",
                    Message = $"{itemDescription} paketiniz başarıyla aktif edildi kanka. Sistemlerinizi hemen kurmaya başlayabilirsiniz!",
                    Type = "success",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { success = true, message = "Ödeme başarıyla tamamlandı, SaaS paketiniz lisanslandı.", newBalance = user.Balance });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest($"Satın alım boru hattında siber hata meydana geldi kanka: {ex.Message}");
            }
        }

        // ========================================================
        // 💀 BOŞALTILMIŞ ESKİ ÖZELLİKLER (REACT ÇÖKMESİN DİYE TEMİZ PLACEHOLDERLAR)
        // ========================================================
        [HttpGet("dashboard")]
        public IActionResult GetDashboardPlaceholder()
        {
            return Ok(new { usedHours = 0, limitHours = 0, accounts = new string[] { }, message = "Bu alan yeni helal SaaS modülü için temizlendi kanka." });
        }

        [HttpPost("account")]
        public IActionResult AddAccountPlaceholder() => Ok(new { success = true, message = "Placeholder hattı aktif." });

        [HttpPost("account/{id}/start")]
        public IActionResult StartPlaceholder(Guid id) => Ok(new { success = true });

        [HttpPost("account/{id}/stop")]
        public IActionResult StopPlaceholder(Guid id) => Ok(new { success = true });

        [HttpDelete("account/{id}")]
        public IActionResult DeletePlaceholder(Guid id) => Ok(new { success = true });
    }

    // --- 🗃️ TERTEMİZ DTO MODEL YAPISI ---
    public class GeneralSaaSPurchaseRequest
    {
        public string PackageType { get; set; } = string.Empty;
    }
}