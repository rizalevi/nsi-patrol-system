using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;

namespace NSI.Domain.Interfaces.Inspection
{
    public interface IInspectionDataLeaderRepository
    {
        Task<List<InspectionDataLeaderListDto>> GetList(InspectionDataLeaderParamDto model);

        Task<List<InspectionDataLeaderListDetailDto>> GetDetailInspection(LeaderParamDetailDto model);

        Task<ResponseApprove> ApproveBatch(InspectionDataLeader model, string userLogin);

        Task<ResponseApprove> ApproveCheckDetail(InspectionDataLeaderDetail model, string userLogin);

    }
}
