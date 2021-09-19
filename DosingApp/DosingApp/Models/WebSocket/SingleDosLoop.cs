using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SingleDosLoop
    {
        public bool? CommandStart { get; set; }
        public bool? CommandStop { get; set; }
        public bool? CommandPause { get; set; }
        public float? RequiredVolume { get; set; }
        public float? DosedVolume { get; set; }
        public float? RatioVolume { get; set; }
        public float? RatioVolumeMicro { get; set; }
    }
}
