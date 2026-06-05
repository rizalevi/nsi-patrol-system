namespace NSI.Domain.DTOs.Master
{
    public class MSEquipmentGroupFilterClusterDto
    {
        public string? ClusterCode { get; set; }
        public string? Description { get; set; }
    }

    public class MSEquipmentGroupFilterPlantDto
    {
        public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public string? Description { get; set; }
    }

    public class MSEquipmentGroupFilterCourseDto
    {
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public string? Title { get; set; }
    }

    // DTO untuk Grid Data (menggabungkan MST_Group dan MST_Course)
    public class MSEquipmentGroupDto
    {
        public string? GroupCode { get; set; }
        public string? BarcodeNo { get; set; }
        public string? Description { get; set; }
        public string? CourseCode { get; set; }
        public string? Title { get; set; } // Join dari MST_Course
        public DateTime? RegisterDate { get; set; }
        public string? RegisterUser { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string? LastUser { get; set; }
    }

    public class MSEquipmentGroupParameterDto
    {
        public string? GroupCode { get; set; }
        public string? BarcodeNo { get; set; }
        public string? Description { get; set; }
        public string? CourseCode { get; set; }
        public string? PlantCode { get; set; }
        public string? Title { get; set; } // Join dari MST_Course
        public DateTime? RegisterDate { get; set; }
        public string? RegisterUser { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string? LastUser { get; set; }
    }

}