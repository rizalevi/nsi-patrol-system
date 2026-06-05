using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Master;
using NSI.Domain.Models.Master; 

namespace NSI.Api.Controllers
{
    [ApiController]
    [Route("api/msmobile")]
    public class MSMobileController : CommonController
    {
        private readonly MSMobileRepositories _service; 

        public MSMobileController(MSMobileRepositories service)
        {
            _service = service;
        }

        [HttpGet("msmobilegetdata")]
        public async Task<IActionResult> GetAll(DateTime? lastSync)
        {
            var data = await _service.GetAll(lastSync);

            return Ok(new
            {
                success = true,
                data = data
            });
        }

        [HttpPost("upset")]
        public async Task<IActionResult> Upset([FromBody] MSMobile payload)
        {  
            await _service.Upset(payload);

            return Success(true, "Data saved successfully");
             
        }

    }
}
