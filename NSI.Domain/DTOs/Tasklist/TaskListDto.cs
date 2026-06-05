using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.DTOs.Tasklist
{

    public class TaskListDto
    {

        public string? ClusterCode { get; set; }
        public string? PlantCode { get; set; }
        public string? DocumentNo { get; set; }
        public string? CourseCode { get; set; }
        public string? Title { get; set; }
        public string? GroupCode { get; set; }
        public string? GroupName { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string? Frequency { get; set; }
        public string? InspectorUser { get; set; }
        public DateTime? InspectorDate { get; set; }
        public string? LeaderUser { get; set; }
        public DateTime? LeaderDate { get; set; }
        public string? Status { get; set; }
    }
}
