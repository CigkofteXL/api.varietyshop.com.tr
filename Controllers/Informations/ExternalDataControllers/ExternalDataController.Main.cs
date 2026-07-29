using Microsoft.AspNetCore.Mvc;

namespace api.varietyshop.com.tr.Controllers.Informations
{
    [Route("external")]
    [ApiController]
    public partial class ExternalDataController : ControllerBase
    {
        // Gelecekte buraya IHttpClientFactory veya loglama servisleri enjekte edilebilir kanka
    }
}