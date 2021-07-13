using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.Screen
{
    public class SensorScreen
    {
        public float Raw { get; set; }
        public float RawLowLimit { get; set; }
        public float RawHighLimit { get; set; }
        public float ValueLowLimit { get; set; }
        public float ValueHighLimit { get; set; }
    }
}
