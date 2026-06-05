using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Tasklist
{
    public class TaskGroupUserDto
    {
        public string? UserID { get; set; }
        public string? GroupID { get; set; }
        public string? GroupName { get; set; }
        public string? UserGroupRole { get; set; }
       
    }
}
