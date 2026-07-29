using System.ComponentModel.DataAnnotations;

namespace api.varietyshop.com.tr.DTOs.Economy
{
    public class DepositDto
    {
        [Required]
        [Range(1, 100000, ErrorMessage = "Tutar 1 ile 100.000 arasında olmalıdır.")]
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "ItemSatis"; // Veya Crypto, Havale
    }

    public class WithdrawDto
    {
        [Required]
        [Range(50, 100000, ErrorMessage = "Çekim alt limiti 50₺'dir.")]
        public decimal Amount { get; set; }

        [Required]
        public string Iban { get; set; } = string.Empty;
    }
}