using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;

namespace NSI.Domain.Interfaces.Inspection
{
    public interface IInspectionDataApprovalRepository
    {
        Task<List<InspectionDataApprovalListDto>> GetList(InspectionDataApprovalParamDto model);

        Task<List<InspectionDataApprovalListDetailDto>> GetDetailInspection(ApprovalParamDetailDto model);

        Task<ResponseApprove> ApproveBatch(InspectionDataApprove model, string userLogin);

        Task<ResponseApprove> ApproveDetail(InspectionDataApproveDetail model, string userLogin);

    }
}
