using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Security
{
    public class GroupPrivilegeDto
    {
        public string GroupMenu { get; set; }
        public string MenuID { get; set; }  
        public string MenuName { get; set; }

        public bool AllowAccess { get; set; }
        public bool AllowUpdate { get; set; }
        public bool AllowDelete { get; set; }

    }

   
}
