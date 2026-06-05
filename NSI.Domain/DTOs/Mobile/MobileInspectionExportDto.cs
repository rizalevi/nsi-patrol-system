using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Mobile
{
    public class MobileInspectionExportDto
    {
        public List<HeaderDto> Headers { get; set; } = new();
        public List<DetailDto> Details { get; set; } = new();
        public String Export_User { get; set; } = "";
    }

    public class HeaderDto
    {
        public string InspectionNo { get; set; }
        public DateTime? Inspection_Date { get; set; }
        public string HT_Code { get; set; }
        public string Plant_Code { get; set; }
        public string Course_Code { get; set; }
        public string Group_Code { get; set; }

        public string Inspector_Status { get; set; }
        public string Inspector_User { get; set; }
        public DateTime? Inspector_Date { get; set; }

        public string Shift_Cls { get; set; }
        public string? Notes { get; set; }
        public string Group_Selection { get; set; }
        public string Leader_User { get; set; }
        public string Supervisor_User { get; set; }
        public string? Register_User { get; set; }
        public string? Register_Date { get; set; }
        public string? Last_User { get; set; }
        public string? Last_Update { get; set; } 
    }

    public class DetailDto
    {
        public int SequenceNo { get; set; }
        public string InspectionNo { get; set; }
        public string Inspection_Code { get; set; }
        public string Group_Code { get; set; }

        public string Unit { get; set; }
        public double? Inspection_Result { get; set; }
        public string? Inspection_Result_Option { get; set; }
        public string Status { get; set; }
        public string? Register_User { get; set; }
        public string? Register_Date { get; set; }
        public string? Last_User { get; set; }
        public string? Last_Date { get; set; }
    }
}
