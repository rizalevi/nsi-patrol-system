namespace NSI.Domain.Models.Master
{
    public class MSInspectionPoint
    {
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public string? InspectionCode { get; set; }
        public string? InspectionPoint { get; set; }
        public string? UnitCode { get; set; }
        public string? Minimum { get; set; }
        public string? Maximum { get; set; }
        public string? Option { get; set; }
        public string? Chart { get; set; }
        public string? Status { get; set; }
        public string? UserID { get; set; }
        public string? IPAddress { get; set; } = "";
    }

    public class MSInpectionPointParam
    {
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
    }

    public class MSInpectionPointDetailParam
    {
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public string? InspectionCode { get; set; }
    }
}
