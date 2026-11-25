# 🚀 VarietyShop Core API (`api.varietyshop.com.tr`)
> **Canlı ApplyBAU YBS Portföyü (Harun Durmuş — VarietyShop & Haduva):** [https://varietyshop.com.tr](https://varietyshop.com.tr)



![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![JWT Security](https://img.shields.io/badge/JWT-Protected-black?style=for-the-badge&logo=jsonwebtokens)
![License](https://img.shields.io/badge/Status-Active_Production-brightgreen?style=for-the-badge)

VarietyShop e-ticaret, dijital lisans pazarı ve SaaS platformunun yetkilendirme, finans, envanter ve admin süreçlerini yöneten yüksek performanslı **.NET 10 Web API** arka plan servisidir.

---

## 🛠️ Teknolojik Mimari & Kütüphaneler

* **Framework:** .NET 10.0 (C#)
* **ORM & Database:** Entity Framework Core 10.0.7 (SQLite altyapısı ile)
* **Kimlik Doğrulama:** JWT Bearer (HMAC-SHA512 imzalı), Google OAuth 2.0, Discord OAuth2
* **Güvenlik & Kalkan:** BCrypt.Net-Next (Parola Hashleme), ASP.NET Core Rate Limiting (Kötüye Kullanım Koruması)
* **Mail Motoru:** MailKit (SMTP / HTML E-posta Bildirimleri)
* **Web Scraping:** HtmlAgilityPack (Harici platform istihbaratı için)
* **Entegrasyonlar:** Discord Webhooks (Anlık Satış & Market Uyarıları)
* **Dokümantasyon:** Swagger / OpenAPI (Swashbuckle 10.1.7)

---

## 🔥 Temel Özellikler ve Modüller

### 1. 🔐 Siber Kimlik & Güvenlik (`/auth`)
* **Çoklu Giriş Mimarisi:** Standart E-posta/Parola, Google Sign-In ve Discord OAuth2 desteği.
* **E-Posta Doğrulama:** Kayıt esnasında 6 haneli siber kod ile e-posta onay mekanizması.
* **Akıllı Referans (Affiliate) Sistemi:** Otomatik üretilen referans kodları ile kullanıcı davet takibi.
* **Güvenlik Loglaması:** Oturum açılışlarında IP adresi ve cihaz (User-Agent) kayıtları.
* **Siber Kalkan (Rate Limit):** Kayıt uç noktalarında IP bazlı istek sınırlandırma (15 dakikada maks. 3 deneme).

### 2. 🛒 Market Engine & Dijital Teslimat (`/market`)
* **Anında Dijital Stok Teslimatı:** Satın alımda otomatik stok düşümü ve lisans anahtarı/hesap bilgisinin anlık teslimi.
* **Sepet Siparişi (Cart Checkout):** Çoklu ürün satın alımında veritabanı işlemleri (Transaction Rollback korumalı).
* **Para İadesi (Cashback) & Komisyon:** Ürün bazlı % Para İadesi ve davet eden kullanıcıya %10 referans payı.
* **Flaş İndirimler (Flash Sale):** Süre kısıtlı özel fiyatlandırma tanımları.
* **Değerlendirme Sistemi:** Kullanıcıların aldığı ürünlere yorum/puan bırakması ve admin onay akışı.

### 3. 💰 Ekonomi, Cüzdan & Takas Havuzu (`/economy`)
* **Cüzdan Yönetimi:** Bakiye yükleme (`deposit`) ve IBAN'a para çekme (`withdraw`) talepleri.
* **E-Pin & Varlık Takası:** Kullanıcıların harici platformlardaki kod veya hesaplarını bakiyeye dönüştürme talepleri (Admin onaylı).
* **Promosyon & Kupon Motoru:** Admin tarafından oluşturulan limitli/tek kullanımlık kupon kodlarının bozdurulması.

### 4. ☁️ SaaS Lisanslama (`/saas`)
* VarietyShop bakiyesi üzerinden modüler SaaS servislerine (Discord Bot Kiralama, OBS Widget, Banner Engine vb.) erişim ve kota kiralama altyapısı.

### 5. 🎧 Destek & Bildirim Sistemi (`/support`, `/notifications`)
* **Destek Talepleri (Tickets):** Kullanıcı ve admin arasında öncelik seviyeli mesajlaşma ve bilet kapatma.
* **Sistem Bildirimleri:** Teslimat, bakiye ve onay süreçlerinde anlık panel bildirimleri.

### 6. 🕵️ Harici Veri & İstihbarat (`/external`)
* **İtemSatış Veri Çekici:** `HtmlAgilityPack` kullanılarak harici mağaza puanı, toplam satış sayısı, canlı ilanlar ve yorumların otomatik çekilmesi.

### 7. 👑 Gelişmiş Admin & Sistem Yönetimi (`/admin`)
* **Canlı Dashboard:** Haftalık satış grafikleri, aktif kullanıcı istatistikleri ve stok takibi.
* **Kullanıcı & Bakiye Müdahalesi:** Bakiye manuel override etme (Audit log takipli) ve hesap banlama/kaldırma.
* **Sistem Bakım Modu (Maintenance Toggle):** Uygulamayı tek tıkla bakım moduna alma.

---

## 📐 Proje Klasör Yapısı

```text
api.varietyshop.com.tr/
├── Controllers/
│   ├── Admin/           # Ürün, Stok, Kullanıcı, Sistem ve Kupon Admin kontrolleri
│   ├── Auth/            # Register, Login, OAuth (Google/Discord), Reset Password
│   ├── Economy/         # Cüzdan, Kupon ve Takas Havuzu süreçleri
│   ├── Informations/    # Market, Bildirim, Duyuru ve Harici Veri (Scraping)
│   ├── Profile/         # Kullanıcı profili, güvenlik ve geçmiş kayıtlar
│   ├── SaaS/            # SaaS paket satın alım motoru
│   ├── Support/         # Destek bileti (Ticket) sistemi
│   └── Tools/           # Discord Webhook entegrasyonları
├── Core/
│   └── Entities/        # EF Core Veritabanı Modelleri (User, Product, LicenseKey vb.)
├── Data/
│   └── AppDbContext.cs  # SQLite Veritabanı bağlamı ve ilişki konfigürasyonları
├── DTOs/                # Veri Taşıma Nesneleri (Auth, Economy, Admin vb.)
├── Migrations/          # EF Core Veritabanı Göç Dosyaları
├── Services/            # E-posta motoru ve yardımcı servisler
├── Program.cs           # CORS, JWT, Rate Limiter ve Servis Bağımlılıkları (DI)
└── appsettings.json     # JWT, SMTP, Webhook ve Bağlantı Cümleçleri

```

---

## ⚙️ Kurulum & Yerel Çalıştırma

### Gereksinimler

* [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

### Adımlar

1. Depoyu klonlayın:
```bash
git clone https://github.com/CigkofteXL/api.varietyshop.com.tr.git
cd api.varietyshop.com.tr

```


2. `appsettings.json` dosyasındaki konfigürasyonları ayarlayın (JWT Secret, SMTP şifreleri, Webhook bağlantıları).
3. Veritabanını güncelleyin (SQLite otomatik olarak `varietyshop.db` adıyla oluşacaktır):
```
dotnet ef database update

```


4. Uygulamayı başlatın:
```bash
dotnet run

```


5. Geliştirici ortamında Swagger arayüzüne erişin:
`https://localhost:7216/swagger` veya `http://localhost:5000/swagger`

---

## 🔒 Güvenlik & CORS Politikası

API, önceden tanımlanmış VIP kökenler (**`BoruSecurityPolicy`**) üzerinden gelen istekleri kabul eder. İzin verilen etki alanları:

* `https://varietyshop.com.tr`
* `https://www.wolvesville.com`
* Yerel test portları (`localhost:5173` Vite/React)

---

## 📬 İletişim

* **Geliştirici:** [@CigkofteXL](https://github.com/CigkofteXL)
* **Web Sitesi:** [varietyshop.com.tr](https://varietyshop.com.tr)
