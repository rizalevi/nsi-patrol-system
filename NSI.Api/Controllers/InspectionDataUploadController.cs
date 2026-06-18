using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;
using NSI.Domain.Models.Inspection;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/InspectionDataUpload")]
    public class InspectionDataUploadController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IInspectionDataUploadRepository _repo;

        public InspectionDataUploadController(IInspectionDataUploadRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionDataUploadParamDto payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var results = await _repo.GetList(payload, userLogin);
            return Success(results);
        }


        [HttpPost("getremarksbyleader")]
        public async Task<IActionResult> GetRemarksByLeader([FromBody] InspectionDataUploadParamDto payload)
        {
            var results = await _repo.GetRemarksByLeader(payload);
            return Success(results);
        }

        [HttpPost("approve-inspector")]
        public async Task<IActionResult> ApproveInspector([FromBody] InspectionDataUploadParamDto payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result =  await _repo.ApproveInspector(payload, userLogin);
            return Success(result, "Data submit successfully");
        }

        [HttpPost("getlaststatus")]
        public async Task<IActionResult> GetLastStatus([FromBody] InspectionDataUploadParamDto payload)
        {
            var results = await _repo.GetLastStatus(payload);
            return Success(results);
        }

        [HttpPost("checkstatusleader")]
        public async Task<IActionResult> CheckStatusLeader([FromBody] InspectionDataUploadParamDto payload)
        {
            var results = await _repo.CheckStatusLeader(payload);
            return Success(results);
        }

        [HttpPost("update-detail")]
        public async Task<IActionResult> UpdateDetail([FromBody] InspectionDataUploadUpdateDto payload)
        {
            string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString()??"";

            // Panggil fungsi UpdateDetail di repository yang sudah kita buat sebelumnya
            var result = await _repo.UpdateDetail(payload,clientIp);

            // Kembalikan respons sukses
            return Success(result, "Data updated successfully");
        }

        //[HttpPost("get-image")]
        //public async Task<IActionResult> GetImage([FromBody] InspectionDataUploadUpdateDto payload)
        //{
        //    // Panggil fungsi GetImage di repository
        //    var imageBytes = await _repo.GetImage(payload);

        //    // Mengembalikan byte[] (akan otomatis di-convert menjadi Base64 JSON oleh .NET)
        //    return Success(imageBytes);
        //}

        [HttpPost("get-image")]
        public async Task<IActionResult> GetImage([FromBody] InspectionDataUploadUpdateDto payload)
        {
            // Mengembalikan string (path) dari database
            var imagePath = await _repo.GetImage(payload);
            return Success(imagePath);
        }

    }
}
