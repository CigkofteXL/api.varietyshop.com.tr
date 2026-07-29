using api.varietyshop.com.tr.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace api.varietyshop.com.tr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- SİBER KALKAN (RATE LIMITING) ---
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("RegisterLimit", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(15);
                    opt.PermitLimit = 3;
                    opt.QueueLimit = 0;
                });
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            // 1. Veritabanı Bağlantısı (SQLite)
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


            // 2. Controller Desteği
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // =======================================================
            // 3. 🛡️ AKILLI SIKI YÖNETİM CORS POLİTİKASI (VIP LİSTE) 🛡️
            // =======================================================

            // Asıl izin verilecek VIP listemiz
            var allowedOrigins = new List<string>
            {
                "https://varietyshop.com.tr",
                "https://www.varietyshop.com.tr",
                "https://www.wolvesville.com",
                "https://wolvesville.com",
                "http://varietyshop.com.tr",          // HTTP İhtimalini ekledik
                "http://www.varietyshop.com.tr",      // HTTP İhtimalini ekledik
                "http://localhost:5173",              // 🔥 BÖRÜ: React (Vite) Local Test Portu
                "http://127.0.0.1:5173"   ,            // 🔥 BÖRÜ: React (Vite) Local Test IP Portu
                 "https://localhost:5173",              // 🔥 BÖRÜ: React (Vite) Local Test Portu
                "https://127.0.0.1:5173"               // 🔥 BÖRÜ: React (Vite) Local Test IP Portu
                // ----------------------------------------------------------------------
            };

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("BoruSecurityPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins.ToArray())
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Token/Cookie transferlerine izin ver
                });
            });

            // 4. JWT Authentication (Siber Kimlik Doğrulama)
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                    };
                });

            builder.Services.AddAuthorization();
            // E-Posta motorunu sisteme tanıtıyoruz
            builder.Services.AddScoped<api.varietyshop.com.tr.Services.IMailService, api.varietyshop.com.tr.Services.MailService>();

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Swagger arayüzü
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // DİKKAT: CORS Politikamızı devreye alıyoruz!
            app.UseCors("BoruSecurityPolicy");

            app.UseWebSockets();    

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();

            // Program.cs içinde 'app.Run();' kodundan hemen ÖNCE bunu ekle:
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate(); // Bu sihirli kod, sistemi açarken veritabanını otomatik günceller!
            }

            app.Run();
        }
    }
}