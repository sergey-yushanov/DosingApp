using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class PowderDos
    {
        public int Number { get; set; }
        public virtual Valve Valve { get; set; }
        public virtual Flowmeter Flowmeter { get; set; }
        public virtual PowderDosLoop Loop { get; set; }
    }
}
