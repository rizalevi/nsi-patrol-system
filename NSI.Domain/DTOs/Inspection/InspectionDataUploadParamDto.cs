using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace NSI.Domain.DTOs.Inspection
{
    public class InspectionDataUploadParamDto
    {
        //public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }  
        public string? Shift { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? InspectionNo { get; set; }
        public string? Type { get; set; }  
    }

    public class InspectionDataUploadRemarksDto
    {
        public string? Leader_Remarks { get; set; }
        public string? Leader_SendBack { get; set; }
        public string? Status { get; set; }
        public string? RetVal { get; set; }
    }

    public class InspectionDataUploadUpdateDto
    {
        public string? InspectionCode { get; set; }
        public string? PlantCode { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        public string? InspectionNo { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? InspectionResult { get; set; }
        public string? InspectionResultOption { get; set; }
        public string? Type { get; set; }
        public string? InspectionResultOptionType { get; set; }
        public string? LastUser { get; set; }
         
        [JsonIgnore]
        public IFormFile? FileImage { get; set; }
         
        public byte[]? FileImageBytes { get; set; }
        public string? FileImagePath { get; set; }
        public string? FileNameStr { get; set; }
    }

    public class InspectionImageDto
    {
        public string? FileImagePath { get; set; }
        public string? FileImageName { get; set; }
    }
}
