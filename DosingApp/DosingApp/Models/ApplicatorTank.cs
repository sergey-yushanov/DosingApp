using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class ApplicatorTank
    {
        public int ApplicatorTankId { get; set; }

        public string Name { get; set; }
        public float? Volume { get; set; }

        public int? ApplicatorId { get; set; }
        public virtual Applicator Applicator { get; set; }
        public bool IsUsedTank { get; set; }
    }
}
