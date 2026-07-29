using System.Threading.Tasks;
namespace api.varietyshop.com.tr.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
