﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Valve
    {
        public int Number { get; set; }
        public bool? Command { get; set; }
        public bool? Faulty { get; set; }
        public bool? CommandOpen { get; set; }
        public bool? CommandClose { get; set; }
    }
}
