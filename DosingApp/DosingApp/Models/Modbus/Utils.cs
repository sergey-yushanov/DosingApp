using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.Modbus
{
    public static class Utils
    {
        public static uint SwapBytes(float x)
        {
            // swap adjacent 16-bit blocks
            byte[] bytes = BitConverter.GetBytes(x);
            uint y = BitConverter.ToUInt32(bytes, 0);
            return (y >> 16) | (y << 16);
        }

        public static float SwapBytes(uint x)
        {
            // swap adjacent 16-bit blocks
            x = (x >> 16) | (x << 16);
            byte[] bytes = BitConverter.GetBytes(x);
            return BitConverter.ToSingle(bytes, 0);
        }
    }
}
