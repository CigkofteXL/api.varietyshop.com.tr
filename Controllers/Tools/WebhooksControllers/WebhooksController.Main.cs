using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace api.varietyshop.com.tr.Controllers.Tools
{
    [Route("tools/webhooks")]
    [ApiController]
    public partial class WebhooksController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static readonly HttpClient _httpClient = new HttpClient(); // Dışarıya istek atmak için roketimiz

        public WebhooksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}