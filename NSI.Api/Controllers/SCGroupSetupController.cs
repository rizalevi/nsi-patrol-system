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
    [Route("api/groupsetup")]
    public class SCGroupSetupController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IGroupSetupRepository _repo;
        private readonly IGroupSettingRepository _groupSettingRepo;

        public SCGroupSetupController(IGroupSetupRepository repo, IGroupSettingRepository groupSettingRepo)
        {
            _repo = repo;
            _groupSettingRepo = groupSettingRepo;
        }

        [HttpPost("group-ddl")]
        public async Task<IActionResult> GetGroupDDL()
        {
            var results = await _repo.GetGroupDDL();
            return Success(results);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetList()
        {
            var results = await _repo.GetAll();
            return Success(results);
        }

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] GroupSetupDto payload)
        {
            var result = await _repo.GetData(payload.GroupID);
            return Success(result);

        }

        [HttpPost("save")]
        public async Task<IActionResult> Create([FromBody] GroupSetup payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.Save(payload, userLogin);

            return Success(null, "Data saved successfully");


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


        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] GroupSetup payload)
        {
            //var before = await _addressRepository.Capture(id);
            string? clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _repo.Remove(payload.GroupID, userLogin, clientIp);
            //await _logger.SaveDataLog(new DataLogDto
            //{
            //    DocumentType = "Master Address",
            //    EntityId = id,
            //    ReferenceId = id,
            //    Action = DataLogAction.Delete,
            //    Before = before
            //});

            return Success(null, "Data deleted successfully");
        }

        [HttpPost("privilege")]
        public async Task<IActionResult> GetGroupPrivileges([FromBody] GroupSetupDto payload)
        {
            var results = await _groupSettingRepo.GetAllPrivilege(payload.GroupID);
            return Success(results);
        }

        [HttpPost("course-setting")]
        public async Task<IActionResult> GetGroupCourseSettings([FromBody] GroupSetupDto payload)
        {
            var results = await _groupSettingRepo.GetAllCourseSetting(payload.GroupID, payload.PlantCode);
            return Success(results);

        }
    }
}
