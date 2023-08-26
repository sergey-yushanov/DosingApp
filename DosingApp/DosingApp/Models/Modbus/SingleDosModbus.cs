using DosingApp.Utils;
using NModbus;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DosingApp.Models.Modbus
{
    public static class SingleDosModbus
    {
        public static ushort numberOfPoints = 16;
        public static ushort numberOfFloats = 7;
        public static ushort floatOffset = 2;

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

        public enum ControlWord : ushort
        {
            VADJ_MAN_OPN = (ushort)1,
            VADJ_MAN_CLS = (ushort)2,
            VOL_RST = (ushort)1024,
            
            DRY_ENABLE = (ushort)2048,
            DRY_DISABLE = (ushort)4096
        }

        public enum StatusWord
        {
            VADJ_OPN = (ushort)0,
            VADJ_COM_OPN = (ushort)1,
            VADJ_COM_CLS = (ushort)2,            
        }

        public enum Register
        {
            CW = (ushort)0,
            SW = (ushort)1,
        }

        public enum Register32
        {
            VOL = (ushort)2,
            VOL_RATIO = (ushort)4,
            FLOW = (ushort)6,
            VADJ_SP = (ushort)8,
            VADJ_POS = (ushort)10,
            REQ_VOL = (ushort)12,
            DOSE_VOL = (ushort)14,
        }

        public static ushort GetRegister(ushort singleDosNumber, Register register)
        {
            return (ushort)(Registers.SingleDoses[singleDosNumber - 1] + register);
        }

        public static ushort GetRegister32(ushort singleDosNumber, Register32 register)
        {

            return (ushort)(Registers.SingleDoses[singleDosNumber - 1] + register);
        }

        public static RegisterValue ValveAdjustableOpen(ushort singleDosNumber)
        {
            return new RegisterValue() { Register = GetRegister(singleDosNumber, Register.CW), Value = (ushort)ControlWord.VADJ_MAN_OPN };
        }

        public static RegisterValue ValveAdjustableClose(ushort singleDosNumber)
        {
            return new RegisterValue() { Register = GetRegister(singleDosNumber, Register.CW), Value = (ushort)ControlWord.VADJ_MAN_CLS };
        }

        public static RegisterValue VolumeReset(ushort singleDosNumber)
        {
            return new RegisterValue() { Register = GetRegister(singleDosNumber, Register.CW), Value = (ushort)ControlWord.VOL_RST };
        }

        public static RegisterValue32 VolumeRatio(ushort singleDosNumber, float volumeRatio)
        {
            return new RegisterValue32() { Register = GetRegister32(singleDosNumber, Register32.VOL_RATIO), Value = Record32.Value(volumeRatio) };
        }

        public static RegisterValue32 ValveAdjustableSetpoint(ushort singleDosNumber, float setPoint)
        {
            return new RegisterValue32() { Register = GetRegister32(singleDosNumber, Register32.VADJ_SP), Value = Record32.Value(setPoint) };
        }

        public static RegisterValue32 VolumeRequired(ushort singleDosNumber, float volumeRequired)
        {
            return new RegisterValue32() { Register = GetRegister32(singleDosNumber, Register32.REQ_VOL), Value = Record32.Value(volumeRequired) };
        }
    }
}
