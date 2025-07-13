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
        public static ushort numberOfPoints = 62;
        //public static ushort numberOfFloats = 12;
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

            //COL_DRY_ENABLE = (ushort)64,
            //COL_DRY_DISABLE = (ushort)128,
            //PDOS_DRY_ENABLE = (ushort)64,
            //PDOS_DRY_DISABLE = (ushort)128,

            PUMP_MAN_START = (ushort)256,
            PUMP_MAN_STOP = (ushort)512,
            VLV_MAN_OPN = (ushort)1024,
            VLV_MAN_CLS = (ushort)2048,
            VOL_RST = (ushort)4096,

            MOTHER_LIQUOR_ENABLE = (ushort)8192,
            MOTHER_LIQUOR_DISABLE = (ushort)16384,

            ACK = (ushort)32768
        }

        //public enum ControlWord2 : ushort
        //{
        //    PDOS_DRY_ENABLE = (ushort)1,
        //    PDOS_DRY_DISABLE = (ushort)2
        //}

        public enum StatusWord
        {
            LOOP_ACTIVE = (ushort)0,
            LOOP_PAUSE = (ushort)1,
            LOOP_RUN = (ushort)2,
            LOOP_DONE = (ushort)3,
            PUMP_COM = (ushort)8,
            VLV_COM = (ushort)9,
            COL_DRY_ON = (ushort)10,
            VDOS_DRY_ON = (ushort)11,
            PDOS_DRY_ON = (ushort)12
        }

        public enum Register : ushort
        {
            CW = (ushort)0,
            SW = (ushort)1,
            //CW_2 = (ushort)90,
        }

        public enum Register32 : ushort
        {
            CAR_VOL = 2,
            CAR_VOL_RATIO = 4,
            CAR_FLOW = 6,
            CAR_REQ_VOL = 8,
            CAR_DOSE_VOL = 10,
            CAR_RES_PCT = 12,
            COL_FINE_K11 = 14,
            COL_FINE_K12 = 16,
            COL_FINE_K13 = 18,
            COL_FINE_SP1 = 20,
            VDOS_FINE_K4 = 22,

            COL_DRY = 24,
            VDOS_DRY = 26,
            COL_FILL_REQ_VOL = 28,

            COL_FINE_K21 = 30,
            COL_FINE_K22 = 32,
            COL_FINE_K23 = 34,
            COL_FINE_SP2 = 36,

            COL_FINE_K31 = 38,
            COL_FINE_K32 = 40,
            COL_FINE_K33 = 42,
            COL_FINE_SP3 = 44,

            COL_FINE_VOL_1_2 = 46,
            COL_FINE_VOL_2_3 = 48,

            VDOS_DELAY_VOL = 50,

            PDOS_FINE_K5 = 52,
            PDOS_DELAY_VOL = 54,
            PDOS_DRY = 56,

            CAR_DELTA_VOL = 58,

            AIR_TEMP = 60
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

        public static RegisterValue MotherLiquorEnable()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.MOTHER_LIQUOR_ENABLE };
        }

        public static RegisterValue MotherLiquorDisable()
        {
            return new RegisterValue() { Register = GetRegister(Register.CW), Value = (ushort)ControlWord.MOTHER_LIQUOR_DISABLE };
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

        public static RegisterValue32 CollectorFineK11(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K11), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK12(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K12), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK13(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K13), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineSetPoint1(float setPoint)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_SP1), Value = Record32.Value(setPoint) };
        }

        public static RegisterValue32 VolumeDosFineK4(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.VDOS_FINE_K4), Value = Record32.Value(k) };
        }

        public static RegisterValue32 PowderDosFineK5(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.PDOS_FINE_K5), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorDry(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_DRY), Value = Record32.Value(k) };
        }

        public static RegisterValue32 VolumeDosDry(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.VDOS_DRY), Value = Record32.Value(k) };
        }

        public static RegisterValue32 PowderDosDry(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.PDOS_DRY), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFillReqVol(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FILL_REQ_VOL), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK21(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K21), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK22(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K22), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK23(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K23), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineSetPoint2(float setPoint)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_SP2), Value = Record32.Value(setPoint) };
        }

        public static RegisterValue32 CollectorFineK31(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K31), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK32(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K32), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineK33(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_K33), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineSetPoint3(float setPoint)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_SP3), Value = Record32.Value(setPoint) };
        }

        public static RegisterValue32 CollectorFineVol_1_2(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_VOL_1_2), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CollectorFineVol_2_3(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.COL_FINE_VOL_2_3), Value = Record32.Value(k) };
        }

        public static RegisterValue32 VolumeDosDelayVolume(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.VDOS_DELAY_VOL), Value = Record32.Value(k) };
        }

        public static RegisterValue32 PowderDosDelayVolume(float k)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.PDOS_DELAY_VOL), Value = Record32.Value(k) };
        }

        public static RegisterValue32 CarrierDeltaVolume(float volume)
        {
            return new RegisterValue32() { Register = GetRegister32(Register32.CAR_DELTA_VOL), Value = Record32.Value(volume) };
        }

        //public static RegisterValue32 AirTemperature(float temperature)
        //{
        //    return new RegisterValue32() { Register = GetRegister32(Register32.AIR_TEMP), Value = Record32.Value(temperature) };
        //}
    }
}
