namespace NSI.Domain.DTOs.Master
{
    public class MSInspectionPointDDLDto 
    {
        public string? ChartType { get; set; }
    }

    public class MSInspectionPointMethodDDLDto
    {
        // Combo fields 
        public string? Methode { get; set; }
        public string? MethodeDescription { get; set; }
        public string? Option { get; set; }
        public string? Option_1 { get; set; }
        public string? Option_2 { get; set; }
        
    }

    public class MSInspectionPointUnitDDLDto
    {
        
        public string? UnitCode { get; set; }
        public string? UnitDescription { get; set; }
        public string? MethodeDescription { get; set; }

    }
}
