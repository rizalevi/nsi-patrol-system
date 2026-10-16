using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.Interfaces.Security;
using NSI.Domain.Models.Security;
using System.Data.SqlClient;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/usersetup")]
    public class SCUserSetupController : CommonController
    {
        //private readonly DataLogger _logger;
        //private readonly UserSetupRepositories _service;
        private readonly IUserSetupRepository _repo;


        public SCUserSetupController(IUserSetupRepository repo)
        {
            _repo = repo;
        }

     
        [HttpPost("list")]
        public async Task<IActionResult> ListData()
        {
            var results = await _repo.GetAll();
            return Success(results);
        }

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] UserSetupInfo payload)
        {
            var result = await _repo.GetData(payload.UserID);
            return Success(result);
           
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserSetup payload)
        {

            try
            {
                var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _repo.Create(payload, userLogin);

                return Success(null, "Data saved successfully");
            }
            catch (SqlException ex)
            {
                return Fail(null, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(null, ex.Message);
            }


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

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] UserSetup payload)
        {
            try
            {
                //var before = await _addressRepository.Capture(model.AddressCode);
                var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _repo.Update(payload, userLogin);
                //var after = await _addressRepository.Capture(model.AddressCode);

                //await _logger.SaveDataLog(new DataLogDto
                //{
                //    DocumentType = "Master Address",
                //    EntityId = model.AddressCode,
                //    ReferenceId = model.AddressCode,
                //    Action = DataLogAction.Update,
                //    Before = before,
                //    After = after
                //});
                return Success(null, "Data updated successfully");
            }
            catch (SqlException ex)
            {
                return Fail(null, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(null, ex.Message);
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] UserSetupInfo payload)
        {
            try
            {
                //var before = await _addressRepository.Capture(id);
                string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

                var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _repo.Remove(payload.UserID, userLogin, clientIp);
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
            catch (SqlException ex)
            {
                return Fail(null, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(null, ex.Message);
            }
        }

        [HttpPost("list-setting-course")]
        public async Task<IActionResult> GetGroupCourseSettings([FromBody] UserSettingCourse payload)
        {
            var results = await _repo.GetUserSettingCourse(payload?.UserID);
            return Success(results);
        }

        [HttpPost("submit-setting-course")]
        public async Task<IActionResult> SubmitBatch([FromBody] List<UserSettingCourse> payload)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _repo.SaveUserSettingCourse(payload, userId);

                return Success(null, "Data submit successfully");
            }
            catch (SqlException ex)
            {
                return Fail(null, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(null, ex.Message);
            }
        }



        [HttpPost("get-profile")]
        public async Task<IActionResult> GetProfile([FromBody] UserSetupInfo payload)
        {
            var result = await _repo.GetProfile(payload.UserID);
            return Success(result);

        }

        [HttpPatch("save-profile")]
        public async Task<IActionResult> SaveProfile([FromBody] UserSetup payload)
        {
            try
            {
                var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _repo.SaveProfile(payload, userLogin);

                return Success(null, "save profile successfully");
            }
            catch (SqlException ex)
            {
                return Fail(null, ex.Message);
            }
            catch (Exception ex)
            {
                return Fail(null, ex.Message);
            }
        }
    }
}
