
namespace NSI.Domain.DTOs.InspectionResult
{
    public class InspectionResultParamDto
    {
        public string? CourseCode { get; set; }
        public string? InspectionPointCode { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
      
      

    }

    public class InspectionResultExcelDto
    {
        public string? CourseCode { get; set; }
        public string? InspectionPointCode { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
        //buat export excel
        public string? CourseName { get; set; }
        public string? InspectionPointName { get; set; }

    }

    public class InspectionResultByItemParamDto
    {
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
       

    }

    public class InspectionResultByItemExcelDto
    {
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
        //buat export excel
        public string? CourseName { get; set; }
        public string? GroupName { get; set; }

    }

    public class InspectionResultByGroupParamDto
    {
        public string? ClusterCode { get; set; }
        public string? PlantCode    { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
      

    }

    public class InspectionResultByGroupExcelDto
    {
        public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public DateTime? InspectionDateFrom { get; set; }
        public DateTime? InspectionDateTo { get; set; }
        //buat export excel
        public string? ClusterName { get; set; }
        public string? PlantName { get; set; }

    }

    public class InspectionGetUnitDto
    {
        public string? UnitCode { get; set; }
        public string? UnitName { get; set; }
    }


}
