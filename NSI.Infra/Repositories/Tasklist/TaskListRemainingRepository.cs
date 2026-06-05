using Dapper;
using NSI.Domain.DTOs.Dashboard;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Tasklist;
using NSI.Domain.Interfaces.Inspection;
using NSI.Infra.Datas;
using System.Data;

namespace NSI.Infra.Repositories.Tasklist
{
    public class TaskListRemainingRepository : ITaskListRemainingRepository
    {
        private readonly DbExecutor _dbExecutor;

        public TaskListRemainingRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<TaskGroupUserDto>> GetTaskGroupUser(string pUserID)
        {
            string sp = "sp_WEB_TaskListRemaining_GetTaskGroupUser";
            var result = await _dbExecutor.QueryListAsync<TaskGroupUserDto>(sp, new { UserID = pUserID });
            return result.ToList();
        }

        public async Task<List<SummaryProcessDto>> GetSummary(string pUserID)
        {
            string sp = "sp_WEB_TaskListRemaining_GetTotal";
            var result = await _dbExecutor.QueryListAsync<SummaryProcessDto>(sp, new { UserID = pUserID });
            return result.ToList();
        }

        public async Task<List<TaskListDto>> GetList(string pUserID)
        {
            string sp = "sp_WEB_TaskListRemaining_GetList";
            var result = await _dbExecutor.QueryListAsync<TaskListDto>(sp, new {UserID = pUserID});
            return result.ToList();

        }

        

    }
}