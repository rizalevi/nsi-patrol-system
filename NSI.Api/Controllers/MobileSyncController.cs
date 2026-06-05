using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Repositories.Mobile;
using NSI.Api.Services.Mobile;
using NSI.Domain.Interfaces.Mobile;
using NSI.Infra.Repositories.Mobile;

namespace NSI.Api.Controllers
{
    [ApiController]
    [Route("api/mobile")]
    public class MobileSyncController : CommonController
    {
        private readonly MobileSyncRepositories _service;
        public MobileSyncController(MobileSyncRepositories service)
        {
            _service = service;
        }

        [HttpGet("syncdata")]
        public async Task<IActionResult> GetSync(string htCode, string userLogin, DateTime? lastSync)
        {
            var data = await _service.GetSync(htCode, userLogin, lastSync);

            return Ok(new
            {
                success = true,
                data = data
            });
        }
    }
}
