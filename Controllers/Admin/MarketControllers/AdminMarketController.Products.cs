using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminMarketController : ControllerBase
    {
        // 1. ÜRÜNLERİ LİSTELE
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Category,
                p.Price,
                p.ImageUrl,
                p.FlashSalePrice,
                p.FlashSaleEndDate,
                p.CashbackPercentage,
                Stock = _context.LicenseKeys.Count(l => l.ProductId == p.Id && !l.IsSold)
            }).ToListAsync();

            return Ok(products);
        }

        // 2. YENİ ÜRÜN EKLE
        [HttpPost("product")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Ürün karargah vitrinine eklendi." });
        }

        // 3. ÜRÜN DÜZENLEME DTO VE ENDPOINT'İ
        public class UpdateProductDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }
            public string ImageUrl { get; set; }
            public decimal? FlashSalePrice { get; set; }
            public DateTime? FlashSaleEndDate { get; set; }
            public decimal CashbackPercentage { get; set; }
        }

        [HttpPut("product/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound("Ürün bulunamadı.");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Category = request.Category;
            product.ImageUrl = request.ImageUrl;
            product.FlashSalePrice = request.FlashSalePrice;
            product.FlashSaleEndDate = request.FlashSaleEndDate;
            product.CashbackPercentage = request.CashbackPercentage;

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Ürün bilgileri ve pazarlama ayarları başarıyla güncellendi." });
        }

        // 4. ÜRÜN SİL
        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var relatedKeys = await _context.LicenseKeys.Where(l => l.ProductId == id).ToListAsync();
            _context.LicenseKeys.RemoveRange(relatedKeys);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Ürün ve ona bağlı tüm mühimmat imha edildi." });
        }
    }
}