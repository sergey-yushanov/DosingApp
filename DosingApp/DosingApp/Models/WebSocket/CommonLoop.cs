using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class CommonLoop
    {
        public bool? CommandStart { get; set; }
        public bool? CommandStop { get; set; }
        public bool? CommandPause { get; set; }
        public float? CarrierRequiredVolume { get; set; }
        public float? CarrierDosedVolume { get; set; }
        public float? ValveSetpoint { get; set; }
    }
}
