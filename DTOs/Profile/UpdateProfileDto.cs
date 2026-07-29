using System.ComponentModel.DataAnnotations;

namespace api.varietyshop.com.tr.DTOs.Profile
{
    public class UpdateProfileDto
    {
        [MaxLength(250, ErrorMessage = "Biyografi en fazla 250 karakter olabilir.")]
        public string? Bio { get; set; }

        public string? AvatarUrl { get; set; }
    }
}