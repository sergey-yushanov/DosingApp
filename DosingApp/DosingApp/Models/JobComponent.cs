using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models
{
    public static class JobComponentUnit
    {
        public const string Dry = "кг";
        public const string Liquid = "л";

        public static List<string> GetVolumeList()
        {
            return new List<string>() { Liquid, Dry };
        }

        public static List<string> GetVolumeRateList()
        {
            return new List<string>() { Liquid + "/га", Dry + "/га" };
        }
    }

    public class JobComponent
    {
        public int JobComponentId { get; set; }
        
        public int? JobId { get; set; }
        public virtual Job Job { get; set; }

        public int? ComponentId { get; set; }
        public virtual Component Component { get; set; }

        public int? Order { get; set; }
        public double? Volume { get; set; }
        public double? VolumeRate { get; set; }
        public string Unit { get; set; }
        public string Dispenser { get; set; }

        public string Name { get { return Component?.Name; } }
    }
}
