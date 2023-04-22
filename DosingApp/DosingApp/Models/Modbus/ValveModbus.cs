using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.Modbus
{
    public class ValveModbus
    {
        public ushort Order { get; set; }
        public float RequiredVolume { get; set; }
        public float DosedVolume { get; set; }
    }
}
