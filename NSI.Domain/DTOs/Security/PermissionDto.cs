namespace NSI.Domain.DTOs.Security
{
    public class PermissionDto
    {
        public string MenuId { get; set; }
        public string? MenuName { get; set; }
        public string? StatusAdmin { get; set; }
        public string? AllowAccess { get; set; }
        public string? AllowUpdate { get; set; }
        public string? AllowDelete { get; set; }
        public string? AllowApprove { get; set; }
    }

    public class PermissionParamDto
    {
        public string? UserID { get; set; }
        public string? MenuID { get; set; }
    }
}
