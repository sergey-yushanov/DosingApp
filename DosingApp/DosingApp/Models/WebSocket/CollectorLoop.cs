﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class CollectorLoop
    {
        public virtual List<int> ValveNums { get; set; }
        public virtual List<float> RequiredVolumes { get; set; }
        public virtual List<float> DosedVolumes { get; set; }
    }
}
