﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Report
    {
        public int ReportId { get; set; }

        public DateTime ReportDateTime { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<ReportComponent> ReportComponents { get; set; }
    }
}