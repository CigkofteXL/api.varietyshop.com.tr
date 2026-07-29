using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    public partial class MarketController : ControllerBase
    {
        // URL: POST /market/products/{productId}/buy
        [HttpPost("products/{productId}/buy")]
        [Authorize]
        public async Task<IActionResult> BuyProduct(Guid productId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);
            var product = await _context.Products.FindAsync(productId);

            if (user == null || product == null) return NotFound("Veri hatası.");

            bool isFlashSaleActive = product.FlashSalePrice.HasValue &&
                                     product.FlashSaleEndDate.HasValue &&
                                     product.FlashSaleEndDate > DateTime.UtcNow;

            decimal finalPrice = isFlashSaleActive ? product.FlashSalePrice.Value : product.Price;

            if (user.Balance < finalPrice)
                return BadRequest("Yetersiz bakiye. Karargaha bakiye takviyesi yapın.");

            var license = await _context.LicenseKeys
                .FirstOrDefaultAsync(l => l.ProductId == productId && !l.IsSold);

            if (license == null)
                return BadRequest("Bu ürünün dijital stoğu tükenmiş. Lütfen admin ile iletişime geçin.");

            user.Balance -= finalPrice;

            // REFERANSA %10 PAY VER
            if (user.ReferredById.HasValue)
            {
                var inviter = await _context.Users.FindAsync(user.ReferredById.Value);
                if (inviter != null)
                {
                    decimal commission = finalPrice * 0.10m;
                    inviter.Balance += commission;

                    _context.TransactionRecords.Add(new TransactionRecord
                    {
                        UserId = inviter.Id,
                        Amount = commission,
                        Type = "Affiliate Commission",
                        Status = "Completed",
                        Description = $"Ağınızdaki bir ajan alışveriş yaptı! %10 Siber Komisyon eklendi."
                    });
                }
            }

            decimal cashbackAmount = 0;
            if (product.CashbackPercentage > 0)
            {
                cashbackAmount = (finalPrice * product.CashbackPercentage) / 100;
                user.Balance += cashbackAmount;
            }

            license.IsSold = true;
            license.SoldToUserId = userId;
            license.SoldAt = DateTime.UtcNow;

            var deliveryNotification = new Notification
            {
                UserId = userId,
                ProductId = product.Id,
                Title = $"📦 Siber Teslimat: {product.Name}",
                Message = $"Operasyon başarılı! Satın aldığınız ürünün bilgileri aşağıdadır:\n\n{license.KeyData}",
                Type = "success",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.Notifications.Add(deliveryNotification);

            _context.TransactionRecords.Add(new TransactionRecord
            {
                UserId = userId,
                Amount = -finalPrice,
                Type = "Market Purchase",
                Status = "Completed",
                Description = $"VarietyShop Alışverişi: {product.Name} (Teslimat Başarılı)"
            });

            if (cashbackAmount > 0)
            {
                _context.TransactionRecords.Add(new TransactionRecord
                {
                    UserId = userId,
                    Amount = cashbackAmount,
                    Type = "Cashback",
                    Status = "Completed",
                    Description = $"{product.Name} ürününden %{product.CashbackPercentage} Para İadesi"
                });
            }

            await _context.SaveChangesAsync();

            try
            {
                string emailBody = $@"
                    <div style='background-color:#0a0c12; color:#e4e1e9; padding:30px; border-radius:15px; font-family:sans-serif; border: 1px solid rgba(255,255,255,0.1); max-width: 500px; margin: auto;'>
                        <h2 style='color:#10b981; text-transform:uppercase; letter-spacing: 2px; border-bottom: 1px solid rgba(16,185,129,0.3); padding-bottom: 10px;'>Teslimat Başarılı</h2>
                        <p style='font-size: 16px;'>Komutan <strong>{user.Username}</strong>,</p>
                        <p style='color:#a0a0a0;'><b>{product.Name}</b> siber envanterine eklendi. Cüzdanından {finalPrice} ₺ düşüldü.</p>
                        <div style='background-color: rgba(0,0,0,0.5); padding: 15px; border-radius: 10px; margin: 20px 0; border-left: 3px solid #10b981;'>
                            <p style='margin:0; font-size:12px; color:#888;'><strong>Lisans / Hesap Bilgisi:</strong></p>
                            <p style='margin:10px 0 0 0; font-size:14px; color:#fff; font-family:monospace; white-space: pre-wrap;'>{license.KeyData}</p>
                        </div>
                        <p style='font-size:10px; color:#555; text-align:center; margin-top:30px;'>VarietyShop Siber Otomasyon Sistemleri</p>
                    </div>";

                await _mailService.SendEmailAsync(user.Email, $"📦 Sipariş Teslimatı: {product.Name}", emailBody);
            }
            catch (Exception ex) { Console.WriteLine($"[MAİL HATASI]: {ex.Message}"); }

            return Ok(new
            {
                Message = cashbackAmount > 0
                    ? $"Satın alma başarılı! Teslimat detayı bildirimlerde. (Cüzdanınıza {cashbackAmount} ₺ Cashback eklendi!)"
                    : "Satın alma başarılı! Teslimat detayı bildirim kutunuza (sağ üst) gönderildi.",
                NewBalance = user.Balance
            });
        }

        // URL: POST /market/checkout (SEPET SATIN ALIMI)
        [HttpPost("checkout")]
        [Authorize]
        public async Task<IActionResult> CheckoutCart([FromBody] CartCheckoutRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return Unauthorized("Siber kimlik doğrulanamadı.");
            if (request.Items == null || !request.Items.Any()) return BadRequest("Sepet boş komutanım.");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                decimal totalCost = 0;
                decimal totalCashback = 0;
                var notificationsToSend = new List<Notification>();
                var totalItemsCount = 0;
                string purchasedItemsHtml = "";

                foreach (var item in request.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null) throw new Exception("Sepetteki bazı ürünler karargahtan silinmiş.");

                    bool isFlashSaleActive = product.FlashSalePrice.HasValue &&
                                             product.FlashSaleEndDate.HasValue &&
                                             product.FlashSaleEndDate > DateTime.UtcNow;

                    decimal itemFinalPrice = isFlashSaleActive ? product.FlashSalePrice.Value : product.Price;

                    var availableLicenses = await _context.LicenseKeys
                        .Where(l => l.ProductId == item.ProductId && !l.IsSold)
                        .Take(item.Quantity)
                        .ToListAsync();

                    if (availableLicenses.Count < item.Quantity)
                        throw new Exception($"'{product.Name}' ürününde yetersiz stok! (Kalan: {availableLicenses.Count})");

                    totalCost += (itemFinalPrice * item.Quantity);
                    totalItemsCount += item.Quantity;

                    if (product.CashbackPercentage > 0)
                    {
                        totalCashback += ((itemFinalPrice * product.CashbackPercentage) / 100) * item.Quantity;
                    }

                    foreach (var license in availableLicenses)
                    {
                        license.IsSold = true;
                        license.SoldToUserId = userId;
                        license.SoldAt = DateTime.UtcNow;

                        notificationsToSend.Add(new Notification
                        {
                            UserId = userId,
                            ProductId = product.Id,
                            Title = $"📦 Teslimat: {product.Name}",
                            Message = $"Siparişin tamamlandı! \nLisans/Hesap Bilgisi: {license.KeyData}",
                            Type = "success",
                            IsRead = false,
                            CreatedAt = DateTime.UtcNow
                        });

                        purchasedItemsHtml += $@"
                            <div style='background-color: rgba(0,0,0,0.5); padding: 15px; border-radius: 10px; margin-bottom: 10px; border-left: 3px solid #ffc300;'>
                                <p style='margin:0; font-size:13px; color:#ffc300; font-weight:bold;'>{product.Name}</p>
                                <p style='margin:5px 0 0 0; font-size:13px; color:#fff; font-family:monospace; white-space: pre-wrap;'>{license.KeyData}</p>
                            </div>";
                    }
                }

                if (user.Balance < totalCost)
                    throw new Exception($"Yetersiz bakiye! Toplam: {totalCost} ₺, Cüzdanınız: {user.Balance} ₺");

                user.Balance -= totalCost;
                user.Balance += totalCashback;

                if (user.ReferredById.HasValue)
                {
                    var inviter = await _context.Users.FindAsync(user.ReferredById.Value);
                    if (inviter != null)
                    {
                        decimal commission = totalCost * 0.10m;
                        inviter.Balance += commission;

                        _context.TransactionRecords.Add(new TransactionRecord
                        {
                            UserId = inviter.Id,
                            Amount = commission,
                            Type = "Affiliate Commission",
                            Status = "Completed",
                            Description = $"Ağınızdaki bir ajan toplu alışveriş yaptı! %10 Siber Komisyon eklendi."
                        });
                    }
                }

                _context.Notifications.AddRange(notificationsToSend);

                _context.TransactionRecords.Add(new TransactionRecord
                {
                    UserId = userId,
                    Amount = -totalCost,
                    Type = "Cart Checkout",
                    Status = "Completed",
                    Description = $"VarietyShop Toplu Alışveriş: {totalItemsCount} Ürün Teslim Edildi."
                });

                if (totalCashback > 0)
                {
                    _context.TransactionRecords.Add(new TransactionRecord
                    {
                        UserId = userId,
                        Amount = totalCashback,
                        Type = "Cashback",
                        Status = "Completed",
                        Description = $"Sepet Alışverişinden Toplam Para İadesi"
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                try
                {
                    string emailBody = $@"
                        <div style='background-color:#0a0c12; color:#e4e1e9; padding:30px; border-radius:15px; font-family:sans-serif; border: 1px solid rgba(255,255,255,0.1); max-width: 500px; margin: auto;'>
                            <h2 style='color:#ffc300; text-transform:uppercase; letter-spacing: 2px; border-bottom: 1px solid rgba(255,195,0,0.3); padding-bottom: 10px;'>Toplu Teslimat Başarılı</h2>
                            <p style='font-size: 16px;'>Komutan <strong>{user.Username}</strong>,</p>
                            <p style='color:#a0a0a0;'>Sepetindeki <b>{totalItemsCount}</b> adet ürün siber envanterine eklendi. Cüzdanından toplam {totalCost} ₺ düşüldü.</p>
                            <h3 style='color:#fff; font-size:14px; margin-top:20px;'>Ürünlerin ve Lisansların:</h3>
                            {purchasedItemsHtml}
                            <p style='font-size:10px; color:#555; text-align:center; margin-top:30px;'>VarietyShop Siber Otomasyon Sistemleri</p>
                        </div>";

                    await _mailService.SendEmailAsync(user.Email, $"📦 VarietyShop Toplu Sipariş Teslimatı", emailBody);
                }
                catch (Exception ex) { Console.WriteLine($"[MAİL HATASI]: {ex.Message}"); }

                return Ok(new
                {
                    Message = totalCashback > 0
                        ? $"İşlem Tamam! Ürünleriniz 'Bildirimler' ikonuna gönderildi. (Cüzdanınıza {totalCashback} ₺ Cashback yansıdı!)"
                        : "İşlem Tamam! Ürünleriniz 'Bildirimler' (Zil) ikonuna gönderildi.",
                    NewBalance = user.Balance
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
    }

    public class CartCheckoutRequest
    {
        public List<CartItemDto> Items { get; set; }
    }

    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}