using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.InspectionResult
{
    public class InspectionResultAnalysisDto
    {
      
        public string? InspectionCode { get; set; }
        public string? InspectionPoint { get; set; }
        public DateTime? Date { get; set; }
        public string? Frequency { get; set; }
        public string? Value { get; set; }
    }

    public class InspectionResultAnalysisChartDto
    {
        public DateTime? InspectionDate { get; set; }
        public string? ShiftNo { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? InspectionResult { get; set; }

        public decimal? MaxValue { get; set; }
        public string? Label { get; set; }

    }
}
