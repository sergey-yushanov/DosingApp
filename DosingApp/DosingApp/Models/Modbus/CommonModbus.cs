using DosingApp.Utils;
using NModbus;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DosingApp.Models.Modbus
{
    public static class CommonModbus
    {
        public class Record
        {
            public Register Register { get; set; }
            public ushort ReadValue { get; set; }
        }

        public static class Record32
        {
            public static Register32 Register { get; set; }

            public static float Value(uint value) { return Utils.SwapBytes(value); }
            public static uint Value(float value) { return Utils.SwapBytes(value); }
        }

        public enum ValveControl
        {
            OPN,
            CLS
        }

        public enum ControlWord : ushort
        {
            LOOP_MAN_START = (ushort)1,
            LOOP_MAN_STOP = (ushort)2,
            LOOP_MAN_PAUSE = (ushort)4,
            LOOP_MAN_CONT = (ushort)8,
            LOOP_MAN_CLEAR = (ushort)16,
            LOOP_MAN_PASS = (ushort)32,

            PUMP_MAN_START = (ushort)256,
            PUMP_MAN_STOP = (ushort)512,
            VLV_MAN_OPN = (ushort)1024,
            VLV_MAN_CLS = (ushort)2048,
            VOL_RST = (ushort)4096,

            ACK = (ushort)32768
        }

        public enum StatusWord
        {
            PUMP_FAULTY,
            VLV_FAULTY
        }

        public enum Register
        {
            CW = (ushort)0,
            SW = (ushort)1,
        }

        public enum Register32
        {
            VOL = 2,
            VOL_RATIO = 4,
            FLOW = 6,
            REQ_VOL = 8,
            DOSE_VOL = 10
        }

        private static ushort GetRegister(Register register)
        {
            return (ushort)(Registers.Common + register);
        }

        private static ushort GetRegister32(Register32 register)
        {

            return (ushort)(Registers.Common + register);
        }

        public static RegisterValue Ack()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.ACK };
        }

        public static RegisterValue PumpStart()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.PUMP_MAN_START };
        }

        public static RegisterValue PumpStop()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.PUMP_MAN_STOP };
        }

        public static RegisterValue ValveOpen()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.VLV_MAN_OPN };
        }

        public static RegisterValue ValveClose()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.VLV_MAN_CLS };
        }

        public static RegisterValue VolumeReset()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.VOL_RST };
        }

        public static RegisterValue32 VolumeRatio(float volumeRatio)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.VOL_RATIO), Value = Record32.Value(volumeRatio) };
        }

        public static RegisterValue32 VolumeRequired(float volumeRequired)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.REQ_VOL), Value = Record32.Value(volumeRequired) };
        }
    }
}
