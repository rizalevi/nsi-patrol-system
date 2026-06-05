using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataLeaderListDetailDto
    {
        public string? Group { get; set; }
        public string? InspectionPoint { get; set; }
        public string? Unit { get; set; }
        public string? Minimum { get; set; }
        public string? Maximum { get; set; }
        public string? InspectionResult { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; } //inspector
        public string? RemarksLeader { get; set; }
        public string? RemarksSendBack { get; set; }


    }
}
