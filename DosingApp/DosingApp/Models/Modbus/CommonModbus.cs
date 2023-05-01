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
        public static ushort numberOfPoints = 24;
        public static ushort numberOfFloats = 11;
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
            LOOP_MAN_START = (ushort)1,
            LOOP_MAN_STOP = (ushort)2,
            LOOP_MAN_PAUSE = (ushort)4,
            LOOP_MAN_CONT = (ushort)8,
            LOOP_MAN_CLEAR = (ushort)16,
            LOOP_MAN_PASS = (ushort)32,

            COL_DRY_ENABLE = (ushort)64,
            COL_DRY_DISABLE = (ushort)128,

            PUMP_MAN_START = (ushort)256,
            PUMP_MAN_STOP = (ushort)512,
            VLV_MAN_OPN = (ushort)1024,
            VLV_MAN_CLS = (ushort)2048,
            VOL_RST = (ushort)4096,

            VDOS_DRY_ENABLE = (ushort)8192,
            VDOS_DRY_DISABLE = (ushort)16384,

            ACK = (ushort)32768
        }

        public enum StatusWord
        {
            PUMP_COM = (ushort)8,
            VLV_COM = (ushort)9,
            COL_DRY_ON = (ushort)10,
            VDOS_DRY_ON = (ushort)11
        }

        public enum Register : ushort
        {
            CW = (ushort)0,
            SW = (ushort)1,
        }

        public enum Register32 : ushort
        {
            CAR_VOL = 2,
            CAR_VOL_RATIO = 4,
            CAR_FLOW = 6,
            CAR_REQ_VOL = 8,
            CAR_DOSE_VOL = 10,
            CAR_RES_PCT = 12,
            COL_FINE_K1 = 14,
            COL_FINE_K2 = 16,
            COL_FINE_K3 = 18,
            COL_FINE_SP = 20,
            VDOS_FINE_K4 = 22,

            COL_DRY = 24,
            VDOS_DRY = 26
        }

        public static ushort GetRegister(Register register)
        {
            return (ushort)(Registers.Common + register);
        }

        public static ushort GetRegister32(Register32 register)
        {

            return (ushort)(Registers.Common + register);
        }

        public static RegisterValue LoopStart()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.LOOP_MAN_START };
        }

        public static RegisterValue LoopStop()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.LOOP_MAN_STOP };
        }

        public static RegisterValue LoopPause()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.LOOP_MAN_PAUSE };
        }

        public static RegisterValue LoopContinue()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.LOOP_MAN_CONT };
        }

        public static RegisterValue LoopClear()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.LOOP_MAN_CLEAR };
        }

        public static RegisterValue LoopPass()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.LOOP_MAN_PASS };
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
            return new RegisterValue32() { Register = GetRegister32(Register32.CAR_VOL_RATIO), Value = Record32.Value(volumeRatio) };
        }

        public static RegisterValue32 VolumeRequired(float volumeRequired)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.CAR_REQ_VOL), Value = Record32.Value(volumeRequired) };
        }

        //public static RegisterValue32 VolumeDosed(ushort[] registers)
        //{
        //    Record32.Value( (volumeRequired)
        //    registers[GetRegister32(Register32.CAR_DOSE_VOL)]

        //    return new RegisterValue32() { Register = GetRegister32(Register32.CAR_DOSE_VOL), Value = Record32.Value(volumeRequired) };
        //}

        public static RegisterValue32 Reserve(float reserve)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.CAR_RES_PCT), Value = Record32.Value(reserve) };
        }

        public static RegisterValue32 CollectorFineK1(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K1), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK2(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K2), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK3(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K3), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineSetPoint(float setPoint)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_SP), Value = Record32.Value(setPoint) };
        }

        public static RegisterValue32 VolumeDosFineK4(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.VDOS_FINE_K4), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorDry(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_DRY), Value = Record32.Value(k) };
        }

        public static RegisterValue32 VolumeDosDry(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.VDOS_DRY), Value = Record32.Value(k) };
        }
    }
}
