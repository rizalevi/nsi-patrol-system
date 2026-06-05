using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Security
{
    public class GroupSetupDDLDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }
    }
}
