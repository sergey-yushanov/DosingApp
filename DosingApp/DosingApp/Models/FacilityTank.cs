using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class FacilityTank
    {
        public int FacilityTankId { get; set; }

        public string Name { get; set; }
        public double? Volume { get; set; }

        public int? FacilityId { get; set; }
        public virtual Facility Facility { get; set; }
    }
}
