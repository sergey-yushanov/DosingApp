using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Report
    {
        public int ReportId { get; set; }

        public DateTime ReportDateTime { get; set; }

        public string Code { get; set; }

        public string AssignmentName { get; set; }
        public string AssignmentPlace { get; set; }
        public string AssignmentNote { get; set; }

        public string RecipeName { get; set; }

        public string OperatorName { get; set; }
        public string DriverName { get; set; }

        public bool IsCompleted { get; set; }

        public string DosingTime { get; set; }
        public string VolumeRate { get; set; }
        public string ProcessedArea { get; set; }

        public ICollection<ReportComponent> ReportComponents { get; set; }
    }
}
