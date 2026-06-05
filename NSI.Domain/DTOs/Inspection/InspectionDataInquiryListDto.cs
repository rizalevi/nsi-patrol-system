using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataInquiryListDto
    {
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }
        public string? PlantCode { get; set; }
        //public string? ClusterCode { get; set; }
        public string? CourseCode { get; set; }
        public string? Group { get; set; }
        public string? InspectionPoint { get; set; }
        public string? Unit { get; set; }
        public string? Minimum { get; set; }
        public string? Maximum { get; set; }
        public string? InspectionResult { get; set; }
        public string? Status { get; set; }
        public string? GroupSelection { get; set; }
        public string? InspectorUser { get; set; }
        public string? Notes { get; set; }

    }
}
