using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Repositories.Mobile;

namespace NSI.Api.Controllers
{
    [ApiController]
    [Route("api/mobile")]
    public class MobileLastVersionController : CommonController
    {
        private readonly MobileLastVersionRepositories _service;
        public MobileLastVersionController(MobileLastVersionRepositories service)
        {
            _service = service;
        }

        [HttpGet("lastversionmobile")]
        public async Task<IActionResult> GetVersion()
        {
            var data = await _service.GetVersion();

            return Ok(new
            {
                success = true,
                data = data
            });
        }
    }
}
