using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    public partial class MarketController : ControllerBase
    {
        // URL: GET /market/search
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts([FromQuery] string? category, [FromQuery] string? search)
        {
            var query = _context.Products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

            var products = await query
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    Stock = _context.LicenseKeys.Count(l => l.ProductId == p.Id && !l.IsSold),
                    p.Category,
                    p.ImageUrl,
                    p.FlashSalePrice,
                    p.FlashSaleEndDate,
                    p.CashbackPercentage,
                    AverageRating = _context.ProductReviews
                        .Where(r => r.ProductId == p.Id && r.IsApproved)
                        .Any()
                        ? Math.Round(_context.ProductReviews.Where(r => r.ProductId == p.Id && r.IsApproved).Average(r => r.Rating), 1)
                        : 0,
                    TotalReviews = _context.ProductReviews.Count(r => r.ProductId == p.Id && r.IsApproved)
                })
                .ToListAsync();

            return Ok(products);
        }
    }
}