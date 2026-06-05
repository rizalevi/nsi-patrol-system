using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.InspectionResult
{
    public class UserPlantDto
    {
        public string? UserID { get; set; } 
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }
    }
}
