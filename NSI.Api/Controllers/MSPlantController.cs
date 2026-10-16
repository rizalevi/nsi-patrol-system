using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Master;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Repositories.Master;
using System.Data.SqlClient;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/msplant")]
    public class MSPlantController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly MSPlantRepositories _service;


        public MSPlantController(MSPlantRepositories service)
        {
            _service = service;
        }

     
        [HttpPost("list")]
        public async Task<IActionResult> ListData([FromBody] MSClusterParameterDto payload)
        {
            var results = await _service.GetAll(payload.ClusterCode);
            return Success(results);
        }

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] MSPlant payload)
        {
            var result = await _service.GetData(payload.ClusterCode,payload.PlantCode);
            return Success(result);
           
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MSPlant payload)
        {
            try
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _service.Create(payload, userId);

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
        public async Task<IActionResult> Update([FromBody] MSPlant payload)
        {
            try
            {
                //var before = await _addressRepository.Capture(model.AddressCode);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _service.Update(payload, userId);

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
           
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] MSPlant payload)
        {
            //var before = await _addressRepository.Capture(id);
            // string? clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
                await _service.Remove(payload.ClusterCode, payload.PlantCode, userId, payload.IPAddress);
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

    }
}
