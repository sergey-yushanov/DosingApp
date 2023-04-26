using DosingApp.Utils;
using NModbus;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DosingApp.Models.Modbus
{
    public static class CollectorModbus
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
            VADJ_MAN_OPN = (ushort)1,
            VADJ_MAN_CLS = (ushort)2,
            VLV_1_MAN_OPN = (ushort)4,
            VLV_1_MAN_CLS = (ushort)8,
            VLV_2_MAN_OPN = (ushort)16,
            VLV_2_MAN_CLS = (ushort)32,
            VLV_3_MAN_OPN = (ushort)64,
            VLV_3_MAN_CLS = (ushort)128,
            VLV_4_MAN_OPN = (ushort)256,
            VLV_4_MAN_CLS = (ushort)512,
            VOL_RST = (ushort)1024
        }

        public enum StatusWord
        {
            VADJ_FAULTY,
            VLV_1_FAULTY,
            VLV_2_FAULTY,
            VLV_3_FAULTY,
            VLV_4_FAULTY
        }

        public enum Register
        {
            CW = (ushort)0,
            SW = (ushort)1,
            ORDER = (ushort)8,
            DOSE_ORDER = (ushort)9,
            VLV_1_ORDER = (ushort)14,
            VLV_2_ORDER = (ushort)19,
            VLV_3_ORDER = (ushort)24
        }

        public enum Register32
        {
            VOL = 2,
            VOL_RATIO = 4,
            FLOW = 6,
            VADJ_SP = 10,
            VADJ_POS = 12,
            VLV_1_REQ_VOL = 15,
            VLV_1_DOSE_VOL = 17,
            VLV_2_REQ_VOL = 20,
            VLV_2_DOSE_VOL = 22,
            VLV_3_REQ_VOL = 25,
            VLV_3_DOSE_VOL = 27
        }

        private static ushort GetRegister(ushort number, Register register)
        {
            return (ushort)(Registers.Collectors[number-1] + register);
        }

        private static ushort GetRegister32(ushort number, Register32 register)
        {

            return (ushort)(Registers.Collectors[number-1] + register);
        }

        public static RegisterValue ValveAdjustableOpen(ushort collectorNumber)
        {
            return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VADJ_MAN_OPN };
        }

        public static RegisterValue ValveAdjustableClose(ushort collectorNumber)
        {
            return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VADJ_MAN_CLS };
        }

        public static RegisterValue ValveOpen(ushort collectorNumber, ushort valveNumber)
        {
            switch(valveNumber)
            {
                case 1: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_1_MAN_OPN };
                case 2: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_2_MAN_OPN };
                case 3: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_3_MAN_OPN };
                case 4: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_4_MAN_OPN };
                default: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)0 };
            }
        }

        public static RegisterValue ValveClose(ushort collectorNumber, ushort valveNumber)
        {
            switch (valveNumber)
            {
                case 1: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_1_MAN_CLS };
                case 2: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_2_MAN_CLS };
                case 3: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_3_MAN_CLS };
                case 4: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_4_MAN_CLS };
                default: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)0 };
            }
        }

        public static RegisterValue VolumeReset(ushort collectorNumber)
        {
            return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VOL_RST };
        }

        public static RegisterValue ValveOrder(ushort collectorNumber, ushort valveNumber, ushort order)
        {
            switch (valveNumber)
            {
                case 1: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.VLV_1_ORDER), Value = (ushort)order };
                case 2: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.VLV_2_ORDER), Value = (ushort)order };
                case 3: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.VLV_3_ORDER), Value = (ushort)order };
                default: return new RegisterValue() { Register = 0, Value = (ushort)0 };
            }
        }

        public static RegisterValue32 VolumeRatio(ushort collectorNumber, float volumeRatio)
        {
            return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VOL_RATIO), Value = Record32.Value(volumeRatio) };
        }

        public static RegisterValue32 ValveAdjustableSetpoint(ushort collectorNumber, float setPoint)
        {
            return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VADJ_SP), Value = Record32.Value(setPoint) };
        }

        public static RegisterValue32 VolumeRequired(ushort collectorNumber, ushort valveNumber, float volumeRequired)
        {
            switch (valveNumber)
            {
                case 1: return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VLV_1_REQ_VOL), Value = Record32.Value(volumeRequired) };
                case 2: return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VLV_2_REQ_VOL), Value = Record32.Value(volumeRequired) };
                case 3: return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VLV_3_REQ_VOL), Value = Record32.Value(volumeRequired) };
                default: return new RegisterValue32() { Register = GetRegister32(0, 0), Value = 0 };
            }
        }
    }
}
