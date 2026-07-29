using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using api.varietyshop.com.tr.Core.Entities;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    public partial class MarketController : ControllerBase
    {
        // 1. Ürünün Onaylanmış Yorumlarını Getir (GET)
        [HttpGet("products/{productId}/reviews")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductReviews(Guid productId)
        {
            var reviews = await _context.ProductReviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId && r.IsApproved)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    Username = r.User.Username,
                    AvatarUrl = r.User.AvatarUrl,
                    r.Rating,
                    r.Comment,
                    Date = r.CreatedAt.ToString("dd.MM.yyyy")
                })
                .ToListAsync();

            double averageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 1) : 0;

            return Ok(new
            {
                AverageRating = averageRating,
                TotalReviews = reviews.Count,
                Reviews = reviews
            });
        }

        // 2. Ürüne Yeni Yorum Ekle (POST)
        [HttpPost("products/{productId}/reviews")]
        [Authorize]
        public async Task<IActionResult> AddProductReview(Guid productId, [FromBody] CreateReviewDto request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var existingReview = await _context.ProductReviews
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);

            if (existingReview != null)
                return BadRequest("Bu ürüne zaten bir değerlendirme bıraktınız.");

            if (request.Rating < 1 || request.Rating > 5)
                return BadRequest("Puanlama 1 ile 5 arasında olmalıdır.");

            var review = new ProductReview
            {
                ProductId = productId,
                UserId = userId,
                Rating = request.Rating,
                Comment = request.Comment,
                IsApproved = false // Yeni yorumlar direkt yayınlanmaz, admin bekler.
            };

            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok("Değerlendirmeniz karargaha ulaştı. Yöneticiler onayladıktan sonra vitrinde görünecektir.");
        }

        // 3. VİTRİN: SON ONAYLANMIŞ YORUMLARI GETİR
        [HttpGet("reviews/latest")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestReviews()
        {
            var reviews = await _context.ProductReviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .Where(r => r.IsApproved)
                .OrderByDescending(r => r.CreatedAt)
                .Take(50)
                .Select(r => new
                {
                    r.Id,
                    Username = r.User.Username,
                    AvatarUrl = r.User.AvatarUrl,
                    ProductName = r.Product.Name,
                    r.Rating,
                    r.Comment,
                    Date = r.CreatedAt.ToString("dd.MM.yyyy HH:mm")
                })
                .ToListAsync();

            return Ok(reviews);
        }
    }

    public class CreateReviewDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}