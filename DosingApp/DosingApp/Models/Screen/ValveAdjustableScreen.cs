using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.Screen
{
    public class ValveAdjustableScreen
    {
        public bool Faulty { get; set; }
        public float Setpoint { get; set; }
        public float Position { get; set; }
        public int Overtime { get; set; }
        public float LimitClose { get; set; }
        public float LimitOpen { get; set; }
        public float DeadbandClose { get; set; }
        public float DeadbandOpen { get; set; }
        public float DeadbandPosition { get; set; }
        public float CostClose { get; set; }
        public float CostOpen { get; set; }
        public SensorScreen Sensor { get; set; }

        public ValveAdjustableScreen()
        {
            Sensor = new SensorScreen();
        }

        public string Name(int collectorNumber)
        {
            return collectorNumber.ToString() + "Кл0";
        }
    }
}
