using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Sensor
    {
        public double? Raw { get; set; }
        public double? RawLowLimit { get; set; }
        public double? RawHighLimit { get; set; }
        public double? ValueLowLimit { get; set; }
        public double? ValueHighLimit { get; set; }
    }
}
