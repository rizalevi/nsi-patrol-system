using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inspectiondatainquiry")]
    public class InspectionDataInquiryController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IInspectionDataInquiryRepository _repo;

        public InspectionDataInquiryController(IInspectionDataInquiryRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionDataInquiryParamDto payload)
        {
            var results = await _repo.GetList(payload);
            return Success(results);
        }

        [HttpPost("report-daily")]
        public async Task<IActionResult> GetReport([FromBody] InspectionDataInquiryParamDto payload)
        {
            var results = await _repo.GetReportDaily(payload);
            return Success(results);
        }

        [HttpPost("report-allshift")]
        public async Task<IActionResult> GetReportAllShift([FromBody] InspectionDataInquiryParamDto payload)
        {
            var results = await _repo.GetReportAllShift(payload);
            return Success(results);
        }
    }
}
