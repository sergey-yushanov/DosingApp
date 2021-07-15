using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ValveAdjustable
    {
        // commands
        public bool? CommandOpen { get; set; }
        public bool? CommandClose { get; set; }
        // properties
        public bool? Faulty { get; set; }
        public double? Setpoint { get; set; }
        public double? Position { get; set; }
        public int? Overtime { get; set; }
        public double? LimitClose { get; set; }
        public double? LimitOpen { get; set; }
        public double? DeadbandClose { get; set; }
        public double? DeadbandOpen { get; set; }
        public double? DeadbandPosition { get; set; }
        public double? CostClose { get; set; }
        public double? CostOpen { get; set; }
        public virtual Sensor Sensor { get; set; }
    }
}
