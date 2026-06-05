using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces.Security;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/userpermission")]
    public class PermissionController : CommonController
    {
        private readonly IPermissionUserRepository _repo;


        public PermissionController(IPermissionUserRepository repo)
        {
            _repo = repo;
        }

  
        [HttpPost("get-privilege")]
        public async Task<IActionResult> GetPermission([FromBody] PermissionParamDto model)
        {
            var results = await _repo.GetPermission(model);
            return Success(results);
        }

    }
}
