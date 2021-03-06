﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class TransportTank
    {
        public int TransportTankId { get; set; }

        public string Name { get; set; }
        public double? Volume { get; set; }

        public int? TransportId { get; set; }
        public virtual Transport Transport { get; set; }
    }
}
