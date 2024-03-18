using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace DosingApp.Models
{
    public class ReportComponent
    {
        public int ReportComponentId { get; set; }

        public int? ReportId { get; set; }
        public virtual Report Report { get; set; }

        public string Name { get; set; }
        public string Dispenser { get; set; }

        public double? RequiredVolume { get; set; }
        public double? DosedVolume { get; set; }
    }
}
