namespace NSI.Domain.DTOs.Report
{
    public class TopPlanAbnormalityPlantDto
    {
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? ColorCode { get; set; }
        public int? AbnormalFrequency { get; set; }
    }

    public class TopPlanAbnormalityPlantParamDto
    {
        public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
    }

    public class TopPlanAbnormalityCourseDto
    {
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }
        //public string? ColorCode { get; set; }
        public int? AbnormalFrequency { get; set; }
    }

    public class TopPlanAbnormalityCourseParamDto //byEquipmentGroup
    {
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
    }

    public class TopPlanAbnormalityInspectionDto
    {
        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }

        public string? InspectionCode { get; set; }
        public string? InspectionName { get; set; }
        public int? AbnormalFrequency { get; set; }
        public string? ColorCode { get; set; }
    }

    public class TopPlanAbnormalityInspectionParamDto
    {
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public string? InspectionPointCode { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
    }

}
