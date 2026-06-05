using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.InspectionResult;
using NSI.Domain.Interfaces.InspectionResult;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inspectionresultbyitem")]
    public class InspectionResultSummaryByItemController : CommonController
    {
        private readonly IInspectionResultSummaryByItemRepository _repo;

        public InspectionResultSummaryByItemController(IInspectionResultSummaryByItemRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("get-userplant")]
        public async Task<IActionResult> GetUserPlant([FromBody] string pUserID)
        {
            var results = await _repo.GetUserPlant(pUserID);
            return Success(results);
        }


        [HttpPost("get-data")]
        public async Task<IActionResult> GetList([FromBody] InspectionResultByItemParamDto payload)
        {
            var results = await _repo.GetData(payload);
            return Success(results);
        }
    }
}
