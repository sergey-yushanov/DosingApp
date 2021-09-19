using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Flowmeter
    {
        public double? Flow { get; set; }
        public double? Volume { get; set; }
        public double? PulsesPerLiter { get; set; }
        public bool? NullifyVolume { get; set; }
    }
}
