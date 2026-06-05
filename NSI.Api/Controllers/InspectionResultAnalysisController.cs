using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.InspectionResult;
using NSI.Domain.Interfaces.InspectionResult;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inspectionresultanalysis")]
    public class InspectionResultAnalysisController : CommonController
    {
        private readonly IInspectionResultAnalysisRepository _repo;

        public InspectionResultAnalysisController(IInspectionResultAnalysisRepository repo)
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
        public async Task<IActionResult> GetList([FromBody] InspectionResultParamDto payload)
        {
            var results = await _repo.GetData(payload);
            return Success(results);
        }

        [HttpPost("get-chart")]
        public async Task<IActionResult> GetChart([FromBody] InspectionResultParamDto payload)
        {
            var results = await _repo.GetChart(payload);
            return Success(results);
        }

        [HttpPost("get-unitname")]
        public async Task<IActionResult> GetUnitName([FromBody] InspectionResultParamDto payload)
        {
            var results = await _repo.GetUnitName(payload);
            return Success(results);
        }
    }
}
