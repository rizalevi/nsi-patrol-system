
namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataApprovalListDto
    {

        public string? PlantCode { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }
        //public string? ClusterCode { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public string? DocumentNo { get; set; }

        public string? InspectorStatus { get; set; }

        public string? InspectorUser { get; set; }
        public DateTime? InspectorDate { get; set; }

        public string? LeaderStatus { get; set; }

        public string? LeaderUser { get; set; }
        public DateTime? LeaderDate { get; set; }

        public string? SupervisorStatus { get; set; }

        public string? SupervisorUser { get; set; }
        public DateTime? SupervisorDate { get; set; }
        public string? Status { get; set; }
        public string? Title { get; set; }
        public string? InspectionNo { get; set; }
    }
}
