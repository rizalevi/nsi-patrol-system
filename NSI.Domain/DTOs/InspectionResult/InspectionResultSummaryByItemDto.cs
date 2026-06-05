using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.InspectionResult
{
    public class InspectionResultSummaryByItemDto
    {
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }

        public string? InspectionCode { get; set; }
        public string? InspectionPoint { get; set; }
        public string? Min { get; set; }
        public string? max { get; set; }
        public string? AbnormalFrequency { get; set; }
        public string? ColorCode { get; set; }
    }
}
