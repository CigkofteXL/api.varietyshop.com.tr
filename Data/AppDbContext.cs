using api.varietyshop.com.tr.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.varietyshop.com.tr.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<TransactionRecord> TransactionRecords { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketReply> TicketReplies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; } 
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<LicenseKey> LicenseKeys { get; set; } 
        public DbSet<UserAsset> UserAssets { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<CouponUsage> CouponUsages { get; set; }

        // 🚨 TAKAS SİSTEMİ TABLOSU
        public DbSet<ExchangeRequest> ExchangeRequests { get; set; }

        // İstihbarat ve Loglama
        public DbSet<UserLoginLog> UserLoginLogs { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Takas Talepleri İlişkisi
            modelBuilder.Entity<ExchangeRequest>()
                .HasOne(e => e.User)
                .WithMany() 
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // Email ve Username benzersiz (Unique) olmalı
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            // Destek Sistemi İlişkileri
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Creator)
                .WithMany()
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketReply>()
                .HasOne(tr => tr.Ticket)
                .WithMany(t => t.Replies)
                .HasForeignKey(tr => tr.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketReply>()
                .HasOne(tr => tr.Sender)
                .WithMany()
                .HasForeignKey(tr => tr.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // İlan Yorumları İlişkisi
            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany() 
                .HasForeignKey(pr => pr.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.User)
                .WithMany()
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}