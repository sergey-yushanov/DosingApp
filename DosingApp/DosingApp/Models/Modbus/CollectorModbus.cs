using DosingApp.Utils;
using NModbus;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DosingApp.Models.Modbus
{
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

    //public struct CW
    //{
    //    public bool valveAdjustableOpen { get; set; }
    //    public bool valveAdjustableClose { get; set; }
    //    public bool valve1Open { get; set; }
    //    public bool valve1Close { get; set; }
    //    public bool valve2Open { get; set; }
    //    public bool valve2Close { get; set; }
    //    public bool valve3Open { get; set; }
    //    public bool valve3Close { get; set; }
    //    public bool valve4Open { get; set; }
    //    public bool valve4Close { get; set; }
    //}

    //[StructLayout(LayoutKind.Explicit)]
    //public struct ControlWord
    //{
    //    [FieldOffset(0)]
    //    public CW cw;
    //    [FieldOffset(0)]
    //    public ushort register;
    //}

    public static class ColMod
    {
        private const ushort startAddressOffset = 16600;

        private static ushort GetStartAddress(ushort number)
        {
            return (ushort)(startAddressOffset + 100 * (number - 1));
        }

        private static ushort GetControlWordAddress(ushort number)
        {
            return GetStartAddress(number);
        }

        public static RegisterValue ValveAdjustableOpen(ushort number)
        {
            var controlWord = new union_bit_field_s();
            controlWord.s[0] = true;
            return new RegisterValue() { Register = GetControlWordAddress(number), Value = controlWord.w };
        }

        public static RegisterValue ValveAdjustableClose(ushort number)
        {
            var controlWord = new union_bit_field_s();
            controlWord.s[1] = true;
            return new RegisterValue() { Register = GetControlWordAddress(number), Value = controlWord.w };
        }

        public static uint SwapBytes(uint x)
        {
            // swap adjacent 16-bit blocks
            x = (x >> 16) | (x << 16);
            // swap adjacent 8-bit blocks
            return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        }

        public static RegisterValue32 ValveAdjustableSetpoint(ushort number, float setPoint)
        {
            byte tmpByte;
            byte[] bytes = BitConverter.GetBytes(setPoint);

            tmpByte = bytes[1];
            bytes[1] = bytes[0];
            bytes[0] = tmpByte;

            tmpByte = bytes[3];
            bytes[3] = bytes[2];
            bytes[2] = tmpByte;
            
            Array.Reverse(bytes, 0, bytes.Length);
            uint num = BitConverter.ToUInt32(bytes, 0);
            
            return new RegisterValue32() { Register = (ushort)(GetStartAddress(number) + 10), Value = num };
        }
    }

    public class CollectorModbus
    {
        private const ushort nValves = 3;
        private const ushort startAddressOffset = 16600;
        
        //public ushort Number { get; set; }
        //public ushort StartAddress { get; set; }

        //private union_bit_field_s controlWord { get; set; }

        //public ushort StatusWord { get; set; }
        //public float Volume { get; set; }
        //public float VolumeRatio { get; set; }
        //public float Flow { get; set; }
        //public ushort Order { get; set; }
        //public ushort DoseOrder { get; set; }
        //public float SetPoint { get; set; }
        //public float Position { get; set; }

        //public ValveModbus[] Valves { get; set; }

        //public CollectorModbus(ushort number)
        //{
        //    //controlWord = new union_bit_field_s();

        //    //controlWord = new ControlWord();

        //    // todo: добавить проверку правильности количества коллекторов
        //    this.Number = number;
        //    //this.StartAddress = (ushort)(this.startAddressOffset + 100 * (this.Number - 1));

        //}



        private ushort GetStartAddress(ushort number)
        {
            return (ushort)(startAddressOffset + 100 * (number - 1));
        }

        public ushort GetControlWordAddress(ushort number)
        {
            return this.GetStartAddress(number);
        }

        //public RegisterValue GetValue()
        //{

        //}

        public RegisterValue ValveAdjustableOpen(ushort number)
        {
            var controlWord = new union_bit_field_s();
            controlWord.s[0] = true;
            return new RegisterValue() { Register = GetControlWordAddress(number), Value = controlWord.w };
        }

        public RegisterValue ValveAdjustableClose(ushort number)
        {
            var controlWord = new union_bit_field_s();
            controlWord.s[1] = true;
            return new RegisterValue() { Register = GetControlWordAddress(number), Value = controlWord.w };
        }


        //public void ValveAdjustableOpen(byte slaveId, IModbusMaster modbusMaster)
        //{
        //    this.WriteControlWord(slaveId, modbusMaster, GetStartAddress(), 1);
        //}

        //public void ValveAdjustableClose(byte slaveId, IModbusMaster modbusMaster)
        //{
        //    this.WriteControlWord(slaveId, modbusMaster, GetStartAddress(), 2);
        //}

        //public void WriteSingleRegister(byte slaveId, IModbusMaster modbusMaster, ushort startAddress, ushort value)
        //{
        //    modbusMaster.WriteSingleRegister(slaveId, startAddress, value);
        //    Console.WriteLine($"{DateTime.Now}\tWriteSingleRegister\tHR_{GetStartAddress()} = {value}");
        //}



    }


}
