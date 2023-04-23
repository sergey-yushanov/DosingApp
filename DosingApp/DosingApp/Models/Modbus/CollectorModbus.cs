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

        public enum ValveCommand
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

        public static RegisterValue ValveCommand(ushort collectorNumber, ushort valveNumber)
        {
            return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VADJ_MAN_OPN };
        }

        public static RegisterValue ValveAdjustableOpen(ushort collectorNumber)
        {
            return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VADJ_MAN_OPN };
        }

        public static RegisterValue ValveAdjustableClose(ushort collectorNumber)
        {
            return new RegisterValue() { Register = GetRegister(collectorNumber, Register.CW), Value = (ushort)ControlWord.VADJ_MAN_CLS };
        }



        public static RegisterValue32 ValveAdjustableSetpoint(ushort collectorNumber, float setPoint)
        {
            return new RegisterValue32() { Register = GetRegister32(collectorNumber, Register32.VADJ_SP), Value = Record32.Value(setPoint) };
        }
    }
}
