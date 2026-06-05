using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.Models.Mobile
{
    public class MobileGroupSetupModel
    {
        public string Group_ID { get; set; }
        public string Group_Name { get; set; }
        public string is_inspector { get; set; }
        public string is_checker { get; set; }
        public string is_leader { get; set; }
        public string is_supervisor { get; set; }
        public string Cluster_Code { get; set; }
        public string Plant_Code { get; set; }
        public DateTime? Register_Date { get; set; }
        public string Register_Program { get; set; }
        public string Register_User { get; set; }
        public DateTime? Update_Date { get; set; } 
        public string Update_User { get; set; }
    }
}
