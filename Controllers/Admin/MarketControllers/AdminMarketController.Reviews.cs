using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Controllers.Admin
{
    public partial class AdminMarketController : ControllerBase
    {
        // 8. ONAY BEKLEYEN YORUMLARI GETİR
        [HttpGet("reviews/pending")]
        public async Task<IActionResult> GetPendingReviews()
        {
            var pendingReviews = await _context.ProductReviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .Where(r => !r.IsApproved)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,
                    ProductId = r.Product.Id,
                    ProductName = r.Product.Name,
                    Username = r.User.Username,
                    r.Rating,
                    r.Comment,
                    Date = r.CreatedAt.ToString("dd.MM.yyyy HH:mm")
                })
                .ToListAsync();

            return Ok(pendingReviews);
        }

        // 9. YORUMU ONAYLA
        [HttpPut("reviews/{id}/approve")]
        public async Task<IActionResult> ApproveReview(Guid id)
        {
            var review = await _context.ProductReviews.FindAsync(id);
            if (review == null) return NotFound("Yorum bulunamadı.");

            review.IsApproved = true;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "İstihbarat raporu onaylandı ve market vitrinine eklendi." });
        }

        // 10. UYGUNSUZ YORUMU SİL
        [HttpDelete("reviews/{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var review = await _context.ProductReviews.FindAsync(id);
            if (review == null) return NotFound("Yorum bulunamadı.");

            _context.ProductReviews.Remove(review);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Uygunsuz rapor karargahtan kalıcı olarak imha edildi." });
        }
    }
}