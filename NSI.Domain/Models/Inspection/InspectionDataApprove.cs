
namespace NSI.Domain.Models.Inspection
{
    public class InspectionDataApprove
    {
        public List<InspectionDataApproveDetail>? ApproveBatch { get; set; }
    }

    public class InspectionDataApproveDetail
    {
        public string? InspectionNo { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }
    }
}
