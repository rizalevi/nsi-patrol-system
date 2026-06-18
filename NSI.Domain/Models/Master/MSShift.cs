namespace NSI.Domain.Models.Master
{
    public class MSShift
    {
        public string? Shift_Cls { get; set; }
        public string? Inspection_Time { get; set; }
        public DateTime Register_Date { get; set; }
        public string? Register_User { get; set; }
        public DateTime Last_Update { get; set; }
        public string? Last_User { get; set; }
        public string? Status { get; set; }
        public string? IPAddress { get; set; } = "";
    }
}
