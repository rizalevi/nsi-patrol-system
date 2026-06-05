using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Master;
using NSI.Api.Services.Mobile;
using NSI.Domain.Models.Master;
using NSI.Infra.Repositories.Master;
using System.Data.SqlClient;
using System.Security.Claims;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace NSI.Api.Controllers
{
    [ApiController]
    [Route("api/mobile")]
    public class MobileCheckConnectionController : CommonController
    {
        private readonly MobileCheckConnectionRepositories _service;

        public MobileCheckConnectionController(MobileCheckConnectionRepositories service)
        {
            _service = service;
        }


        [HttpGet("checkconnection")]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetData();
            return Success(result);
        }
    }
}
