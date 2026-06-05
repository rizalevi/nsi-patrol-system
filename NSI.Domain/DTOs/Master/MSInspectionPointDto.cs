using System.Text.Json.Serialization;

namespace NSI.Domain.DTOs.Master
{
    public class MSInspectionPointDto
    {
        public string? InspectionCode { get; set; }
        public string? InspectionPoint { get; set; }
        public string? UnitCode { get; set; }
        public string? UnitDescription { get; set; }
        public string? Methode { get; set; }
        public string? MethodeDescription { get; set; }
        public string? Minimum { get; set; }
        public string? Maximum { get; set; }
        public string? Chart { get; set; }
        public string? Status { get; set; }

        public DateTime RegisterDate { get; set; }
        public string? RegisterUser { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? LastUser { get; set; }

    }

    

}
