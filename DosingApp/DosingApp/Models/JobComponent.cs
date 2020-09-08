using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models
{
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
        public string VolumeUnit { get; set; }
        public string VolumeRateUnit { get; set; }
        public string Dispenser { get; set; }

        public string Name { get { return Component?.Name; } }
        public string VolumeInfo { get { return Volume + " " + VolumeUnit; } }
        public string VolumeRateInfo { get { return "Норма расхода: " + VolumeRate + " " + VolumeRateUnit; } }
        public string ConsistencyInfo { get { return "Форма: " + Component?.Consistency; } }
        public string DispenserInfo { get { return "Дозатор: " + Dispenser; } }
    }
}
