using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Security;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces.Security;
using NSI.Domain.Models.Security;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/changepassword")]
    public class SCChangePasswordController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IChangePasswordRepository _repo;

        public SCChangePasswordController(IChangePasswordRepository repo)
        {
            _repo = repo;
        }

      
        [HttpPost("save")]
        public async Task<IActionResult> Create([FromBody] ChangePassword payload)
        {
            await _repo.Save(payload);

            return Success(null, "Change password successfully");


            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)??"";
            //await _service.Create(model, userId);

            ////var after = await _addressRepository.Capture(model.AddressCode);
            ////await _logger.SaveDataLog(new DataLogDto
            ////{
            ////    DocumentType = "Master Address",
            ////    EntityId = model.AddressCode,
            ////    ReferenceId = model.AddressCode,
            ////    Before = null,
            ////    After = after,
            ////    Action = DataLogAction.Create
            ////});

            //return Success();
        }


    }
}
