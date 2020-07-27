using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    class Mixture
    {
        public int MixtureId { get; set; }
        
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        
        public float Volume { get; set; }
    }
}
