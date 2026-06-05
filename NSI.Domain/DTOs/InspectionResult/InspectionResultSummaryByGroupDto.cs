namespace NSI.Domain.DTOs.InspectionResult
{
    public class InspectionResultSummaryByGroupDto
    {
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }
        public string? AbnormalFrequency { get; set; }
        public string? ColorCode { get; set; }
    }
}
