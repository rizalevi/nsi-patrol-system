using NSI.Domain.Models.Security;

public class GroupSetup
{

    public string GroupID { get; set; }
    public string? GroupName { get; set; }
    public string? ClusterCode { get; set; }
    public string? ClusterName { get; set; }

    public string? PlantCode { get; set; }
    public string? PlantName { get; set; }

    /*approval setting*/
    public string? InspectorApproval { get; set; }
    public string? CheckerApproval { get; set; }

    public string? LeaderApproval { get; set; }
    public string? SupervisorApproval { get; set; }

    public List<GroupPrivilege>? DetailPrivilege { get; set; }
    public List<CourseSetting>? DetailCourse { get; set; }
}

