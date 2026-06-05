using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataDailyReportDto
    {
        public string? CourseCode { get; set; }
        public string? Date { get; set; }
        public string? Title1 { get; set; }
        public string? Title2 { get; set; }
        public string? No_Left { get; set; }
        public string? Group_Left { get; set; }
        public string? InspectionPoint_Left { get; set; }
        public string? Unit_Left { get; set; }
        public string? Min_Left { get; set; }
        public string? Max_Left { get; set; }

        public string? Shift1_Left { get; set; }
        public string? Shift2_Left { get; set; }
        public string? Shift3_Left { get; set; }

        public string? No_Right { get; set; }
        public string? Group_Right { get; set; }
        public string? InspectionPoint_Right { get; set; }
        public string? Unit_Right { get; set; }
        public string? Min_Right { get; set; }
        public string? Max_Right { get; set; }

        public string? Shift1_Right { get; set; }
        public string? Shift2_Right { get; set; }
        public string? Shift3_Right { get; set; }

        public string? HeaderShiftFooter { get; set; }

        public string? RecordedByShift { get; set; }

        public string? CheckedByShift { get; set; }
        public string? NotesShift { get; set; }
        public string? DocumentNo { get; set; }
    }

    public class InspectionDataAllReportDto
    {
        public string? CourseCode { get; set; }
        public string? Date { get; set; }
        public string? Title1 { get; set; }
        public string? Title2 { get; set; }
        public string? No_Left { get; set; }
        public string? Group_Left { get; set; }
        public string? InspectionPoint_Left { get; set; }
        public string? Unit_Left { get; set; }
        public string? Min_Left { get; set; }
        public string? Max_Left { get; set; }

        public string? HeaderShiftI { get; set; }
        public string? HeaderShiftII { get; set; }
        public string? HeaderShiftIII { get; set; }

        public string? Shift1_Left { get; set; }
        public string? Shift2_Left { get; set; }
        public string? Shift3_Left { get; set; }

        public string? No_Right { get; set; }
        public string? Group_Right { get; set; }
        public string? InspectionPoint_Right { get; set; }
        public string? Unit_Right { get; set; }
        public string? Min_Right { get; set; }
        public string? Max_Right { get; set; }

        public string? Shift1_Right { get; set; }
        public string? Shift2_Right { get; set; }
        public string? Shift3_Right { get; set; }


        public string? RecordedByShift1 { get; set; }
        public string? RecordedByShift2 { get; set; }
        public string? RecordedByShift3 { get; set; }

        public string? CheckedByShift1 { get; set; }
        public string? CheckedByShift2 { get; set; }
        public string? CheckedByShift3 { get; set; }

        public string? NotesShift { get; set; }
        public string? DocumentNo { get; set; }
    }
}
