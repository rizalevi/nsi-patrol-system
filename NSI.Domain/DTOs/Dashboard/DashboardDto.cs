
namespace NSI.Domain.DTOs.Dashboard
{
    public class DashboardDto
    {
        public SummaryProcessDto? SummaryProcess { get; set; }
        public SummaryProgressInspectionDto? SummaryProgressInspection { get; set; }
        //public List<ProgressInspectionDetailDto>? ProgressInspectionDetails { get; set; }
        public List<TopPlantAbnormalDto>? TopPlantAbnormal{ get; set; }
        public List<TopEquipmentGroupAbnormalDto>? TopEquipmentGroupAbnormal { get; set; }
        public List<RealtimeShiftDto>? RealtimeShifts { get; set; }
    }

    public class SummaryProgressInspectionDto
    {
        public int? Remaining { get; set; }
        public int? Completed { get; set; }
       // public int? Uncompleted { get; set; }

    }

    public class ProgressInspectionDetailDto
    {
        public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public string? DocumentNo { get; set; }
        public string? CourseCode { get; set; }
        public string? Title { get; set; }
        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }
        public string? InspectorUser { get; set; }
        public string? Status { get; set; }
    }

    public class TopPlantAbnormalDto
    {
        public string? Period { get; set; }
        public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? ColorCode { get; set; }
        public int? Count { get; set; }
    }

    public class TopEquipmentGroupAbnormalDto
    {
        public string? Period { get; set; }
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }
        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }
        public int? Count { get; set; }
    }


    public class RealtimeShiftDto
    {
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }

        public string? ShiftCode { get; set; }
        public string? Time { get; set; }
        public string? Progress { get; set; }

        public int? Problem { get; set; }
        public int? Overdue { get; set; }
        public string? Status { get; set; }
    }

    public class DashboardParamDto
    {
        public string? UserId { get; set; }
    }

    public class ViewDetailProgressParamDto
    {
        public string? UserId { get; set; }
        public string? Type { get; set; }
    }


    public class RealtimeShiftDetailAbnormalDto
    {
        public string? InspectorUser { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? InspectionNo { get; set; }
        public string? CourseName { get; set; }
        public string? GroupName { get; set; }
        public string? Frequency { get; set; }
        public string? InspectionPoint { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
        public decimal? InspectionResult { get; set; }
    }

}