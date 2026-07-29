using System.ComponentModel.DataAnnotations;

namespace api.varietyshop.com.tr.DTOs.Support
{
    public class CreateTicketDto
    {
        [Required, MaxLength(100)]
        public string Subject { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string InitialMessage { get; set; } = string.Empty;
    }

    public class ReplyTicketDto
    {
        [Required, MaxLength(2000)]
        public string Message { get; set; } = string.Empty;
    }
}