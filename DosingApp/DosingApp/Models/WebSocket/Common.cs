using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Common
    {
        public virtual ValveAdjustable ValveAdjustable { get; set; }
        public virtual Flowmeter Flowmeter { get; set; }
        public virtual CommonLoop Loop { get; set; }
    }
}
