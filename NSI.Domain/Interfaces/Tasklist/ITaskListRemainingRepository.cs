using NSI.Domain.DTOs.Dashboard;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Tasklist;

namespace NSI.Domain.Interfaces.Inspection
{
    public interface ITaskListRemainingRepository
    {
        Task<List<TaskGroupUserDto>> GetTaskGroupUser(string pUserID);
        Task<List<SummaryProcessDto>> GetSummary(string pUserID);
        Task<List<TaskListDto>> GetList(string pUserID);


    }
}
