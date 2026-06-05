using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Api.Services.Master;
using NSI.Domain.Models.Master;
using System.Security.Claims;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/mscluster")]
    public class MSClusterController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly MSClusterRepositories _service;


        public MSClusterController (MSClusterRepositories service)
        {
            _service = service;
        }

     
        [HttpPost("list")]
        public async Task<IActionResult> ListData()
        {
            var results = await _service.GetAll();
            return Success(results);
        }

        [HttpPost("detail")]
        public async Task<IActionResult> GetData([FromBody] MSCluster payload)
        {
            var result = await _service.GetData(payload.ClusterCode);
            return Success(result);
           
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] MSCluster payload)
        {

           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _service.Create(payload, userId);

            return Success(null, "Data saved successfully");
           

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
        public async Task<IActionResult> Update([FromBody] MSCluster payload)
        {
            //var before = await _addressRepository.Capture(model.AddressCode);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _service.Update(payload, userId);
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

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] MSCluster payload)
        {
            //var before = await _addressRepository.Capture(id);
            string? clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _service.Remove(payload.ClusterCode, userId, clientIp);
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

    }
}
