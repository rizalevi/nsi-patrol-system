
namespace NSI.Domain.DTOs.Email
{

    public class EmailLoginDto
    {
        public Guid Token { get; set; }
    }

    public class EmailLoginInfoDto
    {
        public Guid RedirectKey { get; set; }

        public string UserID { get; set; } = "";

        public string RedirectAction { get; set; } = "";

        public string Payload { get; set; } = "";

        public bool IsValid { get; set; }

        public int RedirectCount { get; set; }

    }

    public class InspectionPayloadEmail
    {
        public string? ID { get; set; }

        public string? ClusterCode { get; set; }

        public string? PlantCode { get; set; }

        public string? CourseCode { get; set; }

        public string? GroupCode { get; set; }

        public string? InspectionDate { get; set; }

        public string? Frequency { get; set; }
    }
    public class RedirectKeyRequest
    {
        public Guid Token { get; set; }

        public string Type { get; set; } = "";
    }
}
