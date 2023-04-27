using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class VolumeDosLoop
    {
        public bool? CommandStart { get; set; }
        public bool? CommandStop { get; set; }
        public bool? CommandPause { get; set; }
        public double? RequiredVolume { get; set; }
        public double? DosedVolume { get; set; }
        public double? RatioVolume { get; set; }
        public double? RatioVolumeMicro { get; set; }
    }
}
