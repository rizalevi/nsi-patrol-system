using NSI.Domain.DTOs.Inspection;

namespace NSI.Domain.Interfaces.Inspection
{
    public interface IInspectionDataInquiryRepository
    {
        Task<List<InspectionDataInquiryListDto>> GetList(InspectionDataInquiryParamDto model);

        Task<List<InspectionDataDailyReportDto>> GetReportDaily(InspectionDataInquiryParamDto model);

        Task<List<InspectionDataAllReportDto>> GetReportAllShift(InspectionDataInquiryParamDto model);

    }
}
