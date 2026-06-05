using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Security
{
    public class CourseSettingDto
    {
        public string CourseCode { get; set; }
        public string Title { get; set; }   
        public bool Show { get; set; }
    }
}
