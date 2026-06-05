
namespace NSI.Domain.Models.Logger
{
    public class ErrorLogModel
    {
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        public string? UserId { get; set; }
        public string? QueryString { get; set; }
        public string? RequestId { get; set; }
    }
}
