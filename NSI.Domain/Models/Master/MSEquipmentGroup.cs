namespace NSI.Domain.Models.Master
{
    public class MSEquipmentGroup
    {
        public string? GroupCode { get; set; }
        public string? BarcodeNo { get; set; }
        public string? Description { get; set; }
        public string? CourseCode { get; set; }
        public string? RegisterUser { get; set; }
        public string? LastUser { get; set; }
    }

    public class MSGroupParameterDto
    {
        public string CourseCode { get; set; } = "";
        public string PlantCode { get; set; } = "";
    }

    public class MSPlantParameterDto
    {
        public string PlantCode { get; set; } = "";
    }

    //public class MSClusterParameterDto
    //{
    //    public string ClusterCode { get; set; } = "";
    //}
}