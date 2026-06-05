namespace NSI.Domain.DTOs.Master
{
    public class MSDeviceDto
    {
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }
        public string? Description { get; set; }
        public string? PlantCode   { get; set; }
        public string? PlantName { get; set; } 
        public string? SerialNo { get; set; }
        public DateTime RegisterDate { get; set; }
        public string? RegisterUser { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? LastUser { get; set; }

    }
    public class MSDeviceDetailDto
    {
        public string? Check { get; set; }
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }
        public string? PlantCode { get; set; }
        public string? PlantName { get; set; }
        public string? CourseCode { get; set; }
        public string? Title { get; set; }


    }

    public class MSDeviceDDLDto
    {
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }


    }
}
