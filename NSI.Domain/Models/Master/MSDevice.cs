using NSI.Domain.Models.Inspection;

namespace NSI.Domain.Models.Master
{
    /*crud*/
    public class MSDevice
    {
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }
        public string? Description { get; set; }
        public string? PlantCode { get; set; }
        public string? SerialNo { get; set; }
       
    }
    public class MSDeviceDetail
    {
        public string? DeviceCode { get; set; }
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        
      
    }

    public class MSDeviceBatch
    {
        public List<MSDeviceDetail>? SubmitBatch { get; set; }
    }

}
