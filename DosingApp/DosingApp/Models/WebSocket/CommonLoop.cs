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
        public double? CarrierRequiredVolume { get; set; }
        public double? CarrierDosedVolume { get; set; }
        public double? ValveSetpoint { get; set; }
        public double? CarrierReserve { get; set; }
    }
}
