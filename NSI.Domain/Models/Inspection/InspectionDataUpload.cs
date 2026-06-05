using NSI.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.Models.Inspection
{
    public class InspectionDataUpload
    {
        public List<InspectionDataUploadDetail>? ApproveBatch { get; set; }
    }

    public class InspectionDataUploadDetail
    {
        public string? InspectionNo { get; set; }
        public string? CourseCode { get; set; }
        public string? GroupCode { get; set; }
        
    }
}
