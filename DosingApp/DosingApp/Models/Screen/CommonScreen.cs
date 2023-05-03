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

        private double collectorFineK1;
        private double collectorFineK2;
        private double collectorFineK3;
        private double collectorFineSetPoint;
        private double volumeDosFineK4;
        
        private double collectorDry;

        private bool isVolumeDosDry;
        private double volumeDosDry;

        private bool isCollectorFineK1Focused;
        private bool isCollectorFineK2Focused;
        private bool isCollectorFineK3Focused;
        private bool isCollectorFineSetPointFocused;
        private bool isVolumeDosFineK4Focused;
        
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

            if (!IsCollectorFineK1Focused)
                CollectorFineK1 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K1));
            if (!IsCollectorFineK2Focused)
                CollectorFineK2 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K2));
            if (!IsCollectorFineK3Focused)
                CollectorFineK3 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_K3));
            if (!IsCollectorFineSetPointFocused)
                CollectorFineSetPoint = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.COL_FINE_SP));
            if (!IsVolumeDosFineK4Focused)
                VolumeDosFineK4 = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.VDOS_FINE_K4));

            if (!IsVolumeDosDryFocused)
                VolumeDosDry = CommonModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CommonModbus.Register32.VDOS_DRY));
        }

        //public void InitNew(Common common, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(common.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(common.Flowmeter, showSettings);
        //}

        public double CollectorFineK1
        {
            get { return collectorFineK1; }
            set { SetProperty(ref collectorFineK1, value); }
        }

        public double CollectorFineK2
        {
            get { return collectorFineK2; }
            set { SetProperty(ref collectorFineK2, value); }
        }

        public double CollectorFineK3
        {
            get { return collectorFineK3; }
            set { SetProperty(ref collectorFineK3, value); }
        }

        public double CollectorFineSetPoint
        {
            get { return collectorFineSetPoint; }
            set { SetProperty(ref collectorFineSetPoint, value); }
        }

        public double VolumeDosFineK4
        {
            get { return volumeDosFineK4; }
            set { SetProperty(ref volumeDosFineK4, value); }
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

        public bool IsCollectorFineK1Focused
        {
            get { return isCollectorFineK1Focused; }
            set { SetProperty(ref isCollectorFineK1Focused, value); }
        }

        public bool IsCollectorFineK2Focused
        {
            get { return isCollectorFineK2Focused; }
            set { SetProperty(ref isCollectorFineK2Focused, value); }
        }

        public bool IsCollectorFineK3Focused
        {
            get { return isCollectorFineK3Focused; }
            set { SetProperty(ref isCollectorFineK3Focused, value); }
        }

        public bool IsCollectorFineSetPointFocused
        {
            get { return isCollectorFineSetPointFocused; }
            set { SetProperty(ref isCollectorFineSetPointFocused, value); }
        }

        public bool IsVolumeDosFineK4Focused
        {
            get { return isVolumeDosFineK4Focused; }
            set { SetProperty(ref isVolumeDosFineK4Focused, value); }
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
