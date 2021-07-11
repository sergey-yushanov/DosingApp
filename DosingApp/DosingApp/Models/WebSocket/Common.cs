using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Common
    {
        public ValveAdjustable ValveAdjustable { get; set; }
        public Flowmeter Flowmeter { get; set; }
    }
}
