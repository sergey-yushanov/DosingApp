using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Applicator
    {
        public int ApplicatorId { get; set; }
        public string Name { get; set; }
        
        public virtual List<ApplicatorTank> ApplicatorTanks { get; set; }
    }
}
