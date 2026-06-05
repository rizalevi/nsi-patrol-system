using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.InspectionResult;
using NSI.Domain.Interfaces.InspectionResult;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inspectionresultbygroup")]
    public class InspectionResultSummaryByGroupController : CommonController
    {
        private readonly IInspectionResultSummaryByGroupRepository _repo;

        public InspectionResultSummaryByGroupController(IInspectionResultSummaryByGroupRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("get-data")]
        public async Task<IActionResult> GetData([FromBody] InspectionResultByGroupParamDto payload)
        {
            var results = await _repo.GetData(payload);
            return Success(results);
        }
    }
}
