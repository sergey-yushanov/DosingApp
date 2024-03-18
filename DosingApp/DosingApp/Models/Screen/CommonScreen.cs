using DosingApp.Models.Modbus;
using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System;

namespace DosingApp.Models.Screen
{
    public class CommonScreen : BaseViewModel
    {
        public virtual ValveAdjustableScreen ValveAdjustable { get; set; }
        public virtual FlowmeterScreen Flowmeter { get; set; }

        public double? CarrierRequiredVolume { get; set; }
        public double? CarrierDosedVolume { get; set; }

        private double collectorFineK11;
        private double collectorFineK12;
        private double collectorFineK13;
        private double collectorFineSetPoint1;
        private double collectorFineK21;
        private double collectorFineK22;
        private double collectorFineK23;
        private double collectorFineSetPoint2;
        private double collectorFineK31;
        private double collectorFineK32;
        private double collectorFineK33;
        private double collectorFineSetPoint3;
        private double collectorFineVol_1_2;
        private double collectorFineVol_2_3;

        private double volumeDosFineK4;
        private double volumeDosDelayVolume;
        private double collectorFillReqVol;

        private double collectorDry;

        private bool isVolumeDosDry;
        private double volumeDosDry;

        private bool isCollectorFineK11Focused;
        private bool isCollectorFineK12Focused;
        private bool isCollectorFineK13Focused;
        private bool isCollectorFineSetPoint1Focused;
        private bool isCollectorFineK21Focused;
        private bool isCollectorFineK22Focused;
        private bool isCollectorFineK23Focused;
        private bool isCollectorFineSetPoint2Focused;
        private bool isCollectorFineK31Focused;
        private bool isCollectorFineK32Focused;
        private bool isCollectorFineK33Focused;
        private bool isCollectorFineSetPoint3Focused;
        private bool isCollectorFineVol_1_2Focused;
        private bool isCollectorFineVol_2_3Focused;

        private bool isVolumeDosFineK4Focused;
        private bool isVolumeDosDelayVolumeFocused;
        private bool isCollectorFillReqVolFocused;

        private bool isVolumeDosDryFocused;
        private bool isCollectorDryFocused;

        private bool pumpCommand;

        private bool isLoopActive;
        private bool isLoopPause;
        private bool isLoopRun;
        private bool isLoopDone;

        public CommonScreen()
        {
            ValveAdjustable = new ValveAdjustableScreen() { Name = "РегКл" };
            Flowmeter = new FlowmeterScreen();
        }

        public void Update(Common common, bool showSettings)
        {
            ValveAdjustable.Update(common.ValveAdjustable, showSettings);
            Flowmeter.Update(common.Flowmeter, showSettings);

            CarrierDosedVolume = common.Loop.CarrierDosedVolume;
            CarrierRequiredVolume = common.Loop.CarrierRequiredVolume;

            //Console.Write("CarrierDosedVolume: ");
            //Console.WriteLine(CarrierDosedVolume);

            //Console.Write("CarrierRequiredVolume: ");
            //Console.WriteLine(CarrierRequiredVolume);
        }

        public void Update(ushort[] registers)
        {
            union_bit_field_s status = new union_bit_field_s();
            status.w = registers[(int)CommonModbus.Register.SW];
            PumpCommand = status.s[(int)CommonModbus.StatusWord.PUMP_COM];
            IsVolumeDosDry = status.s[(int)CommonModbus.StatusWord.VDOS_DRY_ON];
            IsLoopActive = status.s[(int)CommonModbus.StatusWord.LOOP_ACTIVE];
            IsLoopPause = status.s[(int)CommonModbus.StatusWord.LOOP_PAUSE];
            IsLoopRun = status.s[(int)CommonModbus.StatusWord.LOOP_RUN];
            IsLoopDone = status.s[(int)CommonModbus.StatusWord.LOOP_DONE];


            float flow = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_FLOW));
            float volume = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_VOL));
            float pulsesPerLiter = (float)Flowmeter.PulsesPerLiter;
            if (!Flowmeter.IsPulsesPerLiterFocused)
                pulsesPerLiter = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_VOL_RATIO));
            Flowmeter.Update(flow, volume, pulsesPerLiter);

            CarrierDosedVolume = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_DOSE_VOL));
            CarrierRequiredVolume = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.CAR_REQ_VOL));

            if (!IsCollectorFineK11Focused)
                CollectorFineK11 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K11));
            if (!IsCollectorFineK12Focused)
                CollectorFineK12 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K12));
            if (!IsCollectorFineK13Focused)
                CollectorFineK13 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K13));
            if (!IsCollectorFineSetPoint1Focused)
                CollectorFineSetPoint1 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_SP1));

            if (!IsCollectorFineK21Focused)
                CollectorFineK21 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K21));
            if (!IsCollectorFineK22Focused)
                CollectorFineK22 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K22));
            if (!IsCollectorFineK23Focused)
                CollectorFineK23 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K23));
            if (!IsCollectorFineSetPoint2Focused)
                CollectorFineSetPoint2 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_SP2));

            if (!IsCollectorFineK31Focused)
                CollectorFineK31 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K31));
            if (!IsCollectorFineK32Focused)
                CollectorFineK32 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K32));
            if (!IsCollectorFineK33Focused)
                CollectorFineK33 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K33));
            if (!IsCollectorFineSetPoint3Focused)
                CollectorFineSetPoint3 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_SP3));

            if (!IsCollectorFineVol_1_2Focused)
                CollectorFineVol_1_2 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_VOL_1_2));
            if (!IsCollectorFineVol_2_3Focused)
                CollectorFineVol_2_3 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_VOL_2_3));

            if (!IsVolumeDosFineK4Focused)
                VolumeDosFineK4 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.VDOS_FINE_K4));
            if (!IsVolumeDosDelayVolumeFocused)
                VolumeDosDelayVolume = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.VDOS_DELAY_VOL));

            if (!IsCollectorFillReqVolFocused)
                CollectorFillReqVol = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FILL_REQ_VOL));

            if (!IsVolumeDosDryFocused)
                VolumeDosDry = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.VDOS_DRY));
        }

        //public void InitNew(Common common, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(common.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(common.Flowmeter, showSettings);
        //}

        public double CollectorFineK11
        {
            get { return collectorFineK11; }
            set { SetProperty(ref collectorFineK11, value); }
        }

        public double CollectorFineK12
        {
            get { return collectorFineK12; }
            set { SetProperty(ref collectorFineK12, value); }
        }

        public double CollectorFineK13
        {
            get { return collectorFineK13; }
            set { SetProperty(ref collectorFineK13, value); }
        }

        public double CollectorFineSetPoint1
        {
            get { return collectorFineSetPoint1; }
            set { SetProperty(ref collectorFineSetPoint1, value); }
        }

        public double VolumeDosFineK4
        {
            get { return volumeDosFineK4; }
            set { SetProperty(ref volumeDosFineK4, value); }
        }

        public double VolumeDosDelayVolume
        {
            get { return volumeDosDelayVolume; }
            set { SetProperty(ref volumeDosDelayVolume, value); }
        }

        public double CollectorFillReqVol
        {
            get { return collectorFillReqVol; }
            set { SetProperty(ref collectorFillReqVol, value); }
        }

        public double CollectorDry
        {
            get { return collectorDry; }
            set { SetProperty(ref collectorDry, value); }
        }

        public double VolumeDosDry
        {
            get { return volumeDosDry; }
            set { SetProperty(ref volumeDosDry, value); }
        }

        public double CollectorFineK21
        {
            get { return collectorFineK21; }
            set { SetProperty(ref collectorFineK21, value); }
        }

        public double CollectorFineK22
        {
            get { return collectorFineK22; }
            set { SetProperty(ref collectorFineK22, value); }
        }

        public double CollectorFineK23
        {
            get { return collectorFineK23; }
            set { SetProperty(ref collectorFineK23, value); }
        }

        public double CollectorFineSetPoint2
        {
            get { return collectorFineSetPoint2; }
            set { SetProperty(ref collectorFineSetPoint2, value); }
        }

        public double CollectorFineK31
        {
            get { return collectorFineK31; }
            set { SetProperty(ref collectorFineK31, value); }
        }

        public double CollectorFineK32
        {
            get { return collectorFineK32; }
            set { SetProperty(ref collectorFineK32, value); }
        }

        public double CollectorFineK33
        {
            get { return collectorFineK33; }
            set { SetProperty(ref collectorFineK33, value); }
        }

        public double CollectorFineSetPoint3
        {
            get { return collectorFineSetPoint3; }
            set { SetProperty(ref collectorFineSetPoint3, value); }
        }

        public double CollectorFineVol_1_2
        {
            get { return collectorFineVol_1_2; }
            set { SetProperty(ref collectorFineVol_1_2, value); }
        }

        public double CollectorFineVol_2_3
        {
            get { return collectorFineVol_2_3; }
            set { SetProperty(ref collectorFineVol_2_3, value); }
        }

        public bool IsCollectorFineK11Focused
        {
            get { return isCollectorFineK11Focused; }
            set { SetProperty(ref isCollectorFineK11Focused, value); }
        }

        public bool IsCollectorFineK12Focused
        {
            get { return isCollectorFineK12Focused; }
            set { SetProperty(ref isCollectorFineK12Focused, value); }
        }

        public bool IsCollectorFineK13Focused
        {
            get { return isCollectorFineK13Focused; }
            set { SetProperty(ref isCollectorFineK13Focused, value); }
        }

        public bool IsCollectorFineSetPoint1Focused
        {
            get { return isCollectorFineSetPoint1Focused; }
            set { SetProperty(ref isCollectorFineSetPoint1Focused, value); }
        }

        public bool IsCollectorFineK21Focused
        {
            get { return isCollectorFineK21Focused; }
            set { SetProperty(ref isCollectorFineK21Focused, value); }
        }

        public bool IsCollectorFineK22Focused
        {
            get { return isCollectorFineK22Focused; }
            set { SetProperty(ref isCollectorFineK22Focused, value); }
        }

        public bool IsCollectorFineK23Focused
        {
            get { return isCollectorFineK23Focused; }
            set { SetProperty(ref isCollectorFineK23Focused, value); }
        }

        public bool IsCollectorFineSetPoint2Focused
        {
            get { return isCollectorFineSetPoint2Focused; }
            set { SetProperty(ref isCollectorFineSetPoint2Focused, value); }
        }

        public bool IsCollectorFineK31Focused
        {
            get { return isCollectorFineK31Focused; }
            set { SetProperty(ref isCollectorFineK31Focused, value); }
        }

        public bool IsCollectorFineK32Focused
        {
            get { return isCollectorFineK32Focused; }
            set { SetProperty(ref isCollectorFineK32Focused, value); }
        }

        public bool IsCollectorFineK33Focused
        {
            get { return isCollectorFineK33Focused; }
            set { SetProperty(ref isCollectorFineK33Focused, value); }
        }

        public bool IsCollectorFineSetPoint3Focused
        {
            get { return isCollectorFineSetPoint3Focused; }
            set { SetProperty(ref isCollectorFineSetPoint3Focused, value); }
        }

        public bool IsCollectorFineVol_1_2Focused
        {
            get { return isCollectorFineVol_1_2Focused; }
            set { SetProperty(ref isCollectorFineVol_1_2Focused, value); }
        }

        public bool IsCollectorFineVol_2_3Focused
        {
            get { return isCollectorFineVol_2_3Focused; }
            set { SetProperty(ref isCollectorFineVol_2_3Focused, value); }
        }

        public bool IsVolumeDosFineK4Focused
        {
            get { return isVolumeDosFineK4Focused; }
            set { SetProperty(ref isVolumeDosFineK4Focused, value); }
        }

        public bool IsVolumeDosDelayVolumeFocused
        {
            get { return isVolumeDosDelayVolumeFocused; }
            set { SetProperty(ref isVolumeDosDelayVolumeFocused, value); }
        }

        public bool IsCollectorFillReqVolFocused
        {
            get { return isCollectorFillReqVolFocused; }
            set { SetProperty(ref isCollectorFillReqVolFocused, value); }
        }

        public bool IsCollectorDryFocused
        {
            get { return isCollectorDryFocused; }
            set { SetProperty(ref isCollectorDryFocused, value); }
        }

        public bool IsVolumeDosDry
        {
            get { return isVolumeDosDry; }
            set { SetProperty(ref isVolumeDosDry, value); }
        }

        public bool IsVolumeDosDryFocused
        {
            get { return isVolumeDosDryFocused; }
            set { SetProperty(ref isVolumeDosDryFocused, value); }
        }

        public bool PumpCommand
        {
            get { return pumpCommand; }
            set { SetProperty(ref pumpCommand, value); }
        }

        public bool IsLoopActive
        {
            get { return isLoopActive; }
            set { SetProperty(ref isLoopActive, value); }
        }

        public bool IsLoopPause
        {
            get { return isLoopPause; }
            set { SetProperty(ref isLoopPause, value); }
        }

        public bool IsLoopRun
        {
            get { return isLoopRun; }
            set { SetProperty(ref isLoopRun, value); }
        }

        public bool IsLoopDone
        {
            get { return isLoopDone; }
            set { SetProperty(ref isLoopDone, value); }
        }
    }
}
