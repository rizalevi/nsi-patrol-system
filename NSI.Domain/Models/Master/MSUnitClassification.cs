namespace NSI.Domain.Models.Master
{
    public class MSUnitClassification
    {
        public string? UnitCode { get; set; }
        public string? UnitDescription { get; set; }
        public string? MethodCode { get; set; }
        public string? MethodDescription { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? RegisterDate { get; set; }
        public string? RegisterUser { get; set; }
        public string? LastUpdate { get; set; }
        public string? LastUser { get; set; }
        public string? IPAddress { get; set; } = "";
    }
}