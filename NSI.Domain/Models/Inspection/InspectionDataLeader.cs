using NSI.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.Models.Inspection
{
    public class InspectionDataLeader
    {
        public List<InspectionDataLeaderBatch>? ApproveBatch { get; set; }
    }

    public class InspectionDataLeaderBatch
    {
        public string? InspectionNo { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }

    }

    public class InspectionDataLeaderDetail
    {
        public string? InspectionNo { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }
        public string? Remarks { get; set; }
        public string? RemarksSendBack { get; set; }
        public string? Type { get; set; }
    }

}
