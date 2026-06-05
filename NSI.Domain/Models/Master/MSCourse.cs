
namespace NSI.Domain.Models.Master
{
    public class MSCourse
    {
        public string? PlantCode { get; set; } 
        public string? ClusterCode { get; set; }
        public string? CourseCode { get; set; }
        public string? Title { get; set; }
        public string? DocNo { get; set; }
        public string? CheckType { get; set; }
        public string? ShiftCls { get; set; }
        public string? StatusShift { get; set; }
        public string? RegisterUser { get; set; } 

    } 
    

    public class MSCourseFilterPlant
    {
        public string? ClusterCode { get; set; } 
        public string? PlantCode { get; set; } 

    } 
}