using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataDDLDto
    {
        public string? Code { get; set; }
        public string? Description { get; set; }

    }
	public class InspectionDataUnitDDLDto
    {
        public string? Unit { get; set; }
        public string? Description { get; set; }
        public string? Methode { get; set; }

    }

    public class InspectionDataUnitOptionDDLDto
    {
        public string? OptionValue { get; set; }
        public string? OptionName { get; set; } 

    } 
}
