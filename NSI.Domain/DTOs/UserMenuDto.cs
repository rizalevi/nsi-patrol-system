using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs
{
    public class UserMenuDto
    {
        public string MenuID { get; set; }= ""; 
        public string MenuName { get; set; } = "";
        public string MenuDescription { get; set; } = "";
        public string MenuGroup { get; set; } = "";
        public int GroupIndex { get; set; }

    }
}
