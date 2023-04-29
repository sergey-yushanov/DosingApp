﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DosingApp.Models.Modbus
{
    public static class Registers
    {
        public const ushort Common = 16500;
        public static ushort[] Collectors = { 16600, 16700 };
        public static ushort[] VolumeDoses = { 16800 };
    }

    public class RegisterValue
    {
        public ushort Register { get; set; }
        public ushort Value { get; set; }
    }

    public class RegisterValue32
    {
        public ushort Register { get; set; }
        public uint Value { get; set; }
    }

    struct _union_bit_field_s
    {
        ushort bits;

        public bool this[int i]
        {
            get
            {
                return (bits & (1 << i)) != 0;
            }
            set
            {
                if (value)
                {
                    bits |= (ushort)(1 << i);
                }

                else
                {
                    bits &= (ushort)~(1 << i);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    struct union_bit_field_s
    {
        [FieldOffset(0)]
        public _union_bit_field_s s;
        [FieldOffset(0)]
        public ushort w;
    }
}