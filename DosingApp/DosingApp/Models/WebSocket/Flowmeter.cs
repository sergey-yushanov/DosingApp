using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Flowmeter
    {
        public float Flow { get; set; }
        public float Volume { get; set; }
        public float PulsesPerLiter { get; set; }
    }
}
