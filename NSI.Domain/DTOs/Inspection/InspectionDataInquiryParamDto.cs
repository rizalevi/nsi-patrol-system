
namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataInquiryParamDto
    {
        public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }  
        public string? InspectionPointCode { get; set; }
        public string? Frequency { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
        public string? UserID { get; set; }
    }
}
