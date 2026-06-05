using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Dashboard
{
    public class SummaryProcessDto
    {
        public int? Uncompleted { get; set; }
        public int? SendBack { get; set; }
        public int? Unchecked { get; set; }
        public int? Unapproved { get; set; }
        public int? Completed { get; set; }
    }

   
}
