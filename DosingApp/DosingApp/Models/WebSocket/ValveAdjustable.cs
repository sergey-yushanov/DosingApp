﻿using Newtonsoft.Json;
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
        public float? Setpoint { get; set; }
        public float? Position { get; set; }
        public int? Overtime { get; set; }
        public float? LimitClose { get; set; }
        public float? LimitOpen { get; set; }
        public float? DeadbandClose { get; set; }
        public float? DeadbandOpen { get; set; }
        public float? DeadbandPosition { get; set; }
        public float? CostClose { get; set; }
        public float? CostOpen { get; set; }
        public virtual Sensor Sensor { get; set; }
    }
}
