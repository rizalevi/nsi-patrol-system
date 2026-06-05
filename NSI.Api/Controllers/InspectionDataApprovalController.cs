using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;
using NSI.Domain.Interfaces.Tasklist;
using NSI.Domain.Models.Inspection;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inspectiondataapproval")]
    public class InspectionDataApprovalController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IInspectionDataApprovalRepository _repo;
        private readonly INotificationRepository _notification;

        public InspectionDataApprovalController(IInspectionDataApprovalRepository repo, INotificationRepository notification)
        {
            _repo = repo;
            _notification = notification;
        }


        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionDataApprovalParamDto payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            payload.UserID = userLogin;
            var results = await _repo.GetList(payload);
            return Success(results);
        }

        [HttpPost("list-detail")]
        public async Task<IActionResult> GetDetail([FromBody] ApprovalParamDetailDto payload)
        {
            var results = await _repo.GetDetailInspection(payload);
            return Success(results);
        }

        [HttpPost("approve-batch")]
        public async Task<IActionResult> ApproveBatch([FromBody] InspectionDataApprove payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _repo.ApproveBatch(payload, userLogin);

           // await _notification.

            return Success(result, "Data approve successfully");

        }

        [HttpPost("approve-detail")]
        public async Task<IActionResult> ApproveDetail([FromBody] InspectionDataApproveDetail payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result =  await _repo.ApproveDetail(payload, userLogin);
            return Success(result, "Data approve successfully");
        }

    }
}
