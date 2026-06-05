namespace NSI.Domain.DTOs.Logger
{
    public class ErrorDto
    {
        public string? CreatedAt { get; set; }
        public string? UserId { get; set; }
        public string? Path { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Method { get; set; }
        
    }
}
