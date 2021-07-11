﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class IncomingMessage
    {
        public Common Common { get; set; }
        public DispenserCollector[] DispenserCollectors { get; set; }
    }
}
