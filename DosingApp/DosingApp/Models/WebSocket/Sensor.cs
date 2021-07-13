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
        public float? Raw { get; set; }
        public float? RawLowLimit { get; set; }
        public float? RawHighLimit { get; set; }
        public float? ValueLowLimit { get; set; }
        public float? ValueHighLimit { get; set; }
    }
}
