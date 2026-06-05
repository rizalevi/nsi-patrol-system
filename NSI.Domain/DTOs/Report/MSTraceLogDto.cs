using System;

namespace NSI.Domain.DTOs.Report
{
    public class MSTraceLogDto
    {
        public string? LogDate { get; set; }
        public string? UserID { get; set; }
        public string? UserName { get; set; }
        public string? IPAddress { get; set; }
        public string? ModuleID { get; set; }
        public string? Module { get; set; }
        public string? Action { get; set; }
        public string? EventLog { get; set; }
    }

    public class MSTraceLogComboDto
    {
        public string? ModuleID { get; set; }
        public string? Description { get; set; }
    }

    public class TraceLogExportParameterDto
    {
        public string Module { get; set; } = "";
        public string ModuleName { get; set; } = ""; 
        public string LogDateFrom { get; set; } = "";
        public string LogDateTo { get; set; } = "";
    }
}