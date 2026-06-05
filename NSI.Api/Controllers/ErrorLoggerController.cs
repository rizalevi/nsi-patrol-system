using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.Interfaces.Logger;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/errorlog")]
    public class ErrorLoggerController : CommonController
    {
        private IErrorLogRepository _repo;

        public ErrorLoggerController(IErrorLogRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetList()
        {
            var results = await _repo.GetAll();
            return Success(results);
        }
    }
}
