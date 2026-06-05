using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataUploadListDto
    {  
        public string? Group { get; set; } 
        public string? Description { get; set; }
        public string? Inspection_Code { get; set; }
        public string? Inspection_Point { get; set; }
        public string? unitCode { get; set; }
        public string? Unit { get; set; }
        public string? Methode { get; set; }
        public string? Course_Code { get; set; }
        public string? Minimum { get; set; }
        public string? Maximum { get; set; }
        public string? Inspection_Result { get; set; }
        public string? StatusDesc { get; set; }
        public string? Status { get; set; }
        public string? DocumentNo { get; set; }
        public string? InspectionNo { get; set; }
        public string? PlantCode { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? Shift { get; set; }
        public DateTime? Register_Date { get; set; }
        public string? Register_User { get; set; }
        public DateTime? Last_Update { get; set; }
        public string? Last_User { get; set; } 
    }
}
