
using System.Data;

namespace NSI.Domain.Models.Security
{
    public class GroupPrivilege
    {
        public string GroupID { get; set; }
        public string MenuID { get; set; }
        public bool AllowAccess { get; set; }
        public bool AllowUpdate { get; set; }
        public bool AllowDelete { get; set; }
    }

    public class CourseSetting
    {
        public string GroupID { get; set; }
        public string CourseCode { get; set; }
        public bool Show { get; set; }
    }

  
    
}