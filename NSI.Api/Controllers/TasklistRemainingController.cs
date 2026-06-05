using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.Interfaces.Inspection;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasklist")]
    public class TasklistRemainingController : CommonController
    {
        //private readonly DataLogger _logger;
        private readonly ITaskListRemainingRepository _repo;

        public TasklistRemainingController(ITaskListRemainingRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("getusergroup")]
        public async Task<IActionResult> GetUserGroup([FromBody] string pUserID)
        {
            var results = await _repo.GetTaskGroupUser(pUserID);
            return Success(results);
        }

        [HttpPost("summary")]
        public async Task<IActionResult> Summary([FromBody] string pUserID)
        {
            var results = await _repo.GetSummary(pUserID);
            return Success(results);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetList([FromBody] string pUserID)
        {
            var results = await _repo.GetList(pUserID);
            return Success(results);
        }

       
    }
}
