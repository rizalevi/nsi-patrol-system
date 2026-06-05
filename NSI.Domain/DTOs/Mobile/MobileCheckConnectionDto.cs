using System.Text.Json.Serialization;

namespace NSI.Domain.DTOs.Mobile
{
    public class MobileCheckConnectionDto
    {
        public string Status { get; set; } = null!;
        public string Database { get; set; } = null!; 
        public DateTime Time { get; set; } 
        public string? Message { get; set; }
    }
}
