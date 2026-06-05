using System.Text.Json.Serialization;

namespace NSI.Domain.DTOs.Master
{
    public class MSClusterDto
    {
        public string ClusterCode { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime RegisterDate { get; set; }
        public string? RegisterUser { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? LastUser { get; set; }

    }
}
