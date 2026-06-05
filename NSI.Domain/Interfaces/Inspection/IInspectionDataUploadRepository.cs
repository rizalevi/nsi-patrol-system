using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;

namespace NSI.Domain.Interfaces.Inspection
{
    public interface IInspectionDataUploadRepository
    {
        Task<List<InspectionDataUploadListDto>> GetList(InspectionDataUploadParamDto model,string userLogin); 
        Task<List<InspectionDataUploadRemarksDto>> GetRemarksByLeader(InspectionDataUploadParamDto model);
        Task<ResponseApprove> ApproveInspector(InspectionDataUploadParamDto model, string userLogin); 
        Task<List<InspectionDataUploadRemarksDto>> GetLastStatus(InspectionDataUploadParamDto model);
        Task<InspectionDataUploadRemarksDto> CheckStatusLeader(InspectionDataUploadParamDto model);
        Task<ResponseApprove> UpdateDetail(InspectionDataUploadUpdateDto model, string ipAddress );
        //Task<byte[]> GetImage(InspectionDataUploadUpdateDto model);
        Task<InspectionImageDto> GetImage(InspectionDataUploadUpdateDto model);
    }
}
