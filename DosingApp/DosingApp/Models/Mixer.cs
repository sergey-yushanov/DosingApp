using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    public class Mixer
    {
        public int MixerId { get; set; }
        public string Name { get; set; }

        public int? Collector { get; set; }
        public int? Single { get; set; }
        public int? ThreeWay { get; set; }

        public bool IsUsedMixer { get; set; }
    }
}
