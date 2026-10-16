using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;
using NSI.Domain.Interfaces.Tasklist;
using NSI.Domain.Models.Inspection;
using System.Data.SqlClient;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inspectiondatacheck")]
    public class InspectionDataLeaderController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IInspectionDataLeaderRepository _repo;
        //private readonly INotificationRepository _notification;

        public InspectionDataLeaderController(IInspectionDataLeaderRepository repo)
        {
            _repo = repo;
            //_notification = notification;
        }


        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionDataLeaderParamDto payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            payload.UserID = userLogin;

            var results = await _repo.GetList(payload);
            return Success(results);
        }

        [HttpPost("list-detail")]
        public async Task<IActionResult> GetDetail([FromBody] LeaderParamDetailDto payload)
        {
            var results = await _repo.GetDetailInspection(payload);
            return Success(results);
        }

        [HttpPost("approve-batch")]
        public async Task<IActionResult> ApproveBatch([FromBody] InspectionDataLeader payload)
        {
            try
            {

                var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                var result = await _repo.ApproveBatch(payload, userLogin);

                return Success(result, "Data save successfully");
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

        [HttpPost("approve-detail")]
        public async Task<IActionResult> ApproveDetail([FromBody] InspectionDataLeaderDetail payload)
        {

            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result =  await _repo.ApproveCheckDetail(payload, userLogin);
            return Success(result, "Data save successfully");
        }

    }
}
