using DosingApp.Utils;
using NModbus;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DosingApp.Models.Modbus
{
    public static class PowderDosModbus
    {
        public static ushort numberOfPoints = 12;
        //public static ushort numberOfFloats = 5;
        public static ushort floatOffset = 2;

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
            VLV_MAN_OPN = (ushort)1,
            VLV_MAN_CLS = (ushort)2,
            VOL_RST = (ushort)4,
            PUMP_MAN_START = (ushort)8,
            PUMP_MAN_STOP = (ushort)16,

            PDOS_DRY_ENABLE = (ushort)16384,
            PDOS_DRY_DISABLE = (ushort)32768
        }

        public enum StatusWord
        {
            VLV_COM = (ushort)0,
            DRY_RUN = (ushort)1,
            PUMP_COM = (ushort)2
        }

        public enum Register
        {
            CW = (ushort)0,
            SW = (ushort)1
        }

        public enum Register32
        {
            VOL = 2,
            VOL_RATIO = 4,
            FLOW = 6,
            REQ_VOL = 8,
            DOSE_VOL = 10
        }

        private static ushort GetRegister(ushort number, Register register)
        {
            return (ushort)(Registers.PowderDoses[number-1] + register);
        }

        private static ushort GetRegister32(ushort number, Register32 register)
        {

            return (ushort)(Registers.PowderDoses[number-1] + register);
        }

        public static RegisterValue ValveOpen(ushort powderDosNumber)
        {
            return new RegisterValue() { Register = GetRegister(powderDosNumber, Register.CW), Value = (ushort)ControlWord.VLV_MAN_OPN };
        }

        public static RegisterValue ValveClose(ushort powderDosNumber)
        {
            return new RegisterValue() { Register = GetRegister(powderDosNumber, Register.CW), Value = (ushort)ControlWord.VLV_MAN_CLS };
        }

        public static RegisterValue VolumeReset(ushort powderDosNumber)
        {
            return new RegisterValue() { Register = GetRegister(powderDosNumber, Register.CW), Value = (ushort)ControlWord.VOL_RST };
        }

        public static RegisterValue PumpStart(ushort powderDosNumber)
        {
            return new RegisterValue() { Register = GetRegister(powderDosNumber, Register.CW), Value = (ushort)ControlWord.PUMP_MAN_START };
        }

        public static RegisterValue PumpStop(ushort powderDosNumber)
        {
            return new RegisterValue() { Register = GetRegister(powderDosNumber, Register.CW), Value = (ushort)ControlWord.PUMP_MAN_STOP };
        }

        public static RegisterValue32 VolumeRatio(ushort powderDosNumber, float volumeRatio)
        {
            return new RegisterValue32() { Register = GetRegister32(powderDosNumber, Register32.VOL_RATIO), Value = Record32.Value(volumeRatio) };
        }

        public static RegisterValue32 VolumeRequired(ushort powderDosNumber, float volumeRequired)
        {
            return new RegisterValue32() { Register = GetRegister32(powderDosNumber, Register32.REQ_VOL), Value = Record32.Value(volumeRequired) };
        }

        public static RegisterValue PowderDosDryEnable()
        {
            return new RegisterValue() { Register = GetRegister(1, Register.CW), Value = (ushort)ControlWord.PDOS_DRY_ENABLE };
        }

        public static RegisterValue PowderDosDryDisable()
        {
            return new RegisterValue() { Register = GetRegister(1, Register.CW), Value = (ushort)ControlWord.PDOS_DRY_DISABLE };
        }
    }
}
