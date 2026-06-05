using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inspectiondata")]
    public class InspectionDataDDLController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly IInspectionDataDDLRepository _repo;

        public InspectionDataDDLController(IInspectionDataDDLRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("cluster-ddl")]
        public async Task<IActionResult> GetClusterDDL()
        {
            var results = await _repo.GetClusterDDL();
            return Success(results);
        }

        [HttpPost("plant-ddl")]
        public async Task<IActionResult> GetPlantDDL([FromBody] InspectionDataInquiryParamDto payload)
        {
            var results = await _repo.GetPlantDDL(payload.ClusterCode);
            return Success(results);
        }

        [HttpPost("course-ddl")]
        public async Task<IActionResult> GetCourse([FromBody] InspectionDataInquiryParamDto payload)
        {
            var results = await _repo.GetCourseDDL(payload.PlantCode);
            return Success(results);
        }

        [HttpPost("group-ddl")]
        public async Task<IActionResult> GetGroup([FromBody] InspectionDataInquiryParamDto payload)
        {
            var results = await _repo.GetGroupDDL(payload.CourseCode,payload.PlantCode);
            return Success(results);
        }

        [HttpPost("inspectionpoint-ddl")]
        public async Task<IActionResult> GetInspectionPoint([FromBody] InspectionDataInquiryParamDto payload)
        {
            var results = await _repo.GetInspectionPointDDL(payload.CourseCode, payload.GroupCode);
            return Success(results);
        }

        [HttpPost("frequency-ddl")]
        public async Task<IActionResult> GetShift()
        {
            var results = await _repo.GetFrequencyDDL();
            return Success(results);
        }

        [HttpPost("inspectionpoint-chart-ddl")]
        public async Task<IActionResult> GetInspectionPointForChart([FromBody] InspectionDataInquiryParamDto payload)
        {
            var results = await _repo.GetInspectionPointForChartDDL(payload.CourseCode);
            return Success(results);
        }
		[HttpPost("unit-ddl")]
        public async Task<IActionResult> GetUnit()
        {
            var results = await _repo.GetUnitDDL();
            return Success(results);
        }

        [HttpPost("unitoption-ddl")]
        public async Task<IActionResult> GetUnitOption([FromBody] InspectionDataUploadListDto payload)
        {
            var results = await _repo.GetUnitOptionDDL(payload.unitCode);
            return Success(results);
        }							
    }
}
