﻿using DosingApp.Utils;
using NModbus;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DosingApp.Models.Modbus
{
    public static class CollectorModbus
    {
        public static ushort numberOfPoints = 34;
        //public static ushort numberOfFloats = 11;
        public static ushort floatOffset = 7;

        public class Record
        {
            public static Register Register { get; set; }
            public static ushort Value { get; set; }
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
            VLV_5_MAN_OPN = (ushort)256,
            VLV_5_MAN_CLS = (ushort)512,
            VOL_RST = (ushort)1024,
            VLV_4_MAN_OPN = (ushort)2048,
            VLV_4_MAN_CLS = (ushort)4096

            //DRY_ENABLE = (ushort)2048,
            //DRY_DISABLE = (ushort)4096
        }

        public enum StatusWord
        {
            VADJ_OPN = (ushort)0,
            VADJ_COM_OPN = (ushort)1,
            VADJ_COM_CLS = (ushort)2,
            VLV_1_COM = (ushort)3,
            VLV_2_COM = (ushort)4,
            VLV_3_COM = (ushort)5,
            VLV_5_COM = (ushort)6,
            VLV_4_COM = (ushort)7

            //VLV_1_DRY_RUN = (ushort)7,
            //VLV_2_DRY_RUN = (ushort)8,
            //VLV_3_DRY_RUN = (ushort)9
        }

        public enum Register
        {
            CW = (ushort)0,
            SW = (ushort)1,
            ORDER = (ushort)2,
            DOSE_ORDER = (ushort)3,
            VLV_1_ORDER = (ushort)4,
            VLV_2_ORDER = (ushort)5,
            VLV_3_ORDER = (ushort)6,
            VLV_4_ORDER = (ushort)33
        }

        public enum Register32
        {
            VOL = (ushort)7,
            VOL_RATIO = (ushort)9,
            FLOW = (ushort)11,
            VADJ_SP = (ushort)13,
            VADJ_POS = (ushort)15,
            VLV_1_REQ_VOL = (ushort)17,
            VLV_1_DOSE_VOL = (ushort)19,
            VLV_2_REQ_VOL = (ushort)21,
            VLV_2_DOSE_VOL = (ushort)23,
            VLV_3_REQ_VOL = (ushort)25,
            VLV_3_DOSE_VOL = (ushort)27,
            VLV_4_REQ_VOL = (ushort)29,
            VLV_4_DOSE_VOL = (ushort)31
        }

        public static ushort GetRegister(ushort collectorNumber, Register register)
        {
            return (ushort)(Registers.Collectors[collectorNumber - 1] + register);
        }

        public static ushort GetRegister32(ushort collectorNumber, Register32 register)
        {

            return (ushort)(Registers.Collectors[collectorNumber - 1] + register);
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
                case 5: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_5_MAN_OPN };
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
                case 5: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VLV_5_MAN_CLS };
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
                case 4: return new RegisterValue() { Register = GetRegister(collectorNumber, Register.VLV_4_ORDER), Value = (ushort)order };
                default: return new RegisterValue() { Register = 0, Value = (ushort)0 };
            }
        }

        public static RegisterValue32 VolumeRatio(ushort collectorNumber, float volumeRatio)
        {
            return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VOL_RATIO), Value = Record32.Value(volumeRatio) };
        }

        public static RegisterValue32 ValveAdjustableSetpoint(ushort collectorNumber, float setPoint)
        {
            setPoint = setPoint > 100 ? 100 : setPoint;
            setPoint = setPoint < 0 ? 0 : setPoint;

            return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VADJ_SP), Value = Record32.Value(setPoint) };
        }

        public static RegisterValue32 VolumeRequired(ushort collectorNumber, ushort valveNumber, float volumeRequired)
        {
            switch (valveNumber)
            {
                case 1: return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VLV_1_REQ_VOL), Value = Record32.Value(volumeRequired) };
                case 2: return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VLV_2_REQ_VOL), Value = Record32.Value(volumeRequired) };
                case 3: return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VLV_3_REQ_VOL), Value = Record32.Value(volumeRequired) };
                case 4: return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VLV_4_REQ_VOL), Value = Record32.Value(volumeRequired) };
                default: return new RegisterValue32() { Register = GetRegister32(0, 0), Value = 0 };
            }
        }
    }
}
