using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class OutgoingMessage
    {
        public bool? ShowSettings { get; set; }
        public bool? Ack { get; set; }
        public virtual Common Common { get; set; }
        public virtual List<Collector> Collectors { get; set; }
    }
}