using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class CollectorLoop
    {
        public bool? CommandStart { get; set; }
        public bool? CommandStop { get; set; }
        public bool? CommandPause { get; set; }
        public virtual List<int> ValveNums { get; set; }
        public virtual List<double> RequiredVolumes { get; set; }
        public virtual List<double> DosedVolumes { get; set; }
        
        public double? RatioVolume0 { get; set; }
        public double? RatioVolume1 { get; set; }
        public double? RatioVolume2 { get; set; }
        public double? RatioVolume3 { get; set; }

        public double? Volume1 { get; set; }
        public double? Volume2 { get; set; }
        public double? Volume3 { get; set; }

        public double? Setpoint1 { get; set; }
        public double? Setpoint2 { get; set; }
        public double? Setpoint3 { get; set; }

        public int Number { get; set; }

        public CollectorLoop(int number)
        {
            Number = number;
            ValveNums = new List<int>();
            RequiredVolumes = new List<double>();
            DosedVolumes = new List<double>(4);
        }
    }
}
