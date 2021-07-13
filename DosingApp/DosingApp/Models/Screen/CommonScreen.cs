using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.Screen
{
    public class CommonScreen
    {
        public virtual ValveAdjustableScreen ValveAdjustable { get; set; }
        public virtual FlowmeterScreen Flowmeter { get; set; }
    }
}
