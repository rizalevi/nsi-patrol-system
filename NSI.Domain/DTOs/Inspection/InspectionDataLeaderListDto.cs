namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataLeaderListDto
    {
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }
        public string? CourseCode { get; set; }
        public string? Title { get; set; }

        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }

        public string? InspectorStatus { get; set; }

        public string? InspectorUser { get; set; }
        public DateTime? InspectorDate { get; set; }

        public string? LeaderStatus { get; set; }

        public string? LeaderUser { get; set; }
        public DateTime? LeaderDate { get; set; }
        public string? Status { get; set; }
        public string? InspectionNo { get; set; }
    }
}
