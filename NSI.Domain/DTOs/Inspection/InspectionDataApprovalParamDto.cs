namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataApprovalParamDto
    {
        public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }  
        public string? Frequency { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
        public string? UserID { get; set; }


    }

    public class ApprovalParamDetailDto
    {
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }
    }
}
