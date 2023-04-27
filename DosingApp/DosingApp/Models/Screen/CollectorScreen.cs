using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using DosingApp.Models.Modbus;

namespace DosingApp.Models.Screen
{
    public class CollectorScreen : BaseViewModel
    {
        private const int nValves = 4;  // в коллекторе 4 клапана on/off

        public int Number { get; set; }
        public ObservableCollection<ValveScreen> Valves { get; set; }
        public ValveAdjustableScreen ValveAdjustable { get; set; }
        public FlowmeterScreen Flowmeter { get; set; }
        
        public ObservableCollection<int> ValveNums { get; set; }
        public ObservableCollection<double> RequiredVolumes { get; set; }
        public ObservableCollection<double> DosedVolumes { get; set; }

        private double ratioVolume0;
        private double ratioVolume1;
        private double ratioVolume2;

        private double volume1;
        private double volume2;

        private double setpoint1;
        private double setpoint2;

        private bool isRatioVolume0Focused;
        private bool isRatioVolume1Focused;
        private bool isRatioVolume2Focused;

        private bool isVolume1Focused;
        private bool isVolume2Focused;

        private bool isSetpoint1Focused;
        private bool isSetpoint2Focused;

        public CollectorScreen(int number)
        {
            this.Number = number;
            Valves = new ObservableCollection<ValveScreen>();
            for (int i = nValves; i > 0; i--)
            {
                Valves.Add(new ValveScreen() { Number = i, Name = this.Number.ToString() + "Кл" + i.ToString() });
            }
            ValveAdjustable = new ValveAdjustableScreen() { Name = this.Number.ToString() + "РегКл" };
            Flowmeter = new FlowmeterScreen();

            ValveNums = new ObservableCollection<int>();
            RequiredVolumes = new ObservableCollection<double>();
            DosedVolumes = new ObservableCollection<double>();
            for (int i = 0; i < nValves - 1; i++)
            {
                ValveNums.Add(0);
                RequiredVolumes.Add(0);
                DosedVolumes.Add(0);
            }
        }

        public string Name { get { return "Коллекторный дозатор " + Number.ToString() + " (КД " + Number.ToString() + ")"; } }

        public void Update(Collector collector, bool showSettings)
        {
            for (int i = 0; i < nValves; i++)
            {
                var valve = Valves.FirstOrDefault(v => v.Number == collector.Valves[i].Number);
                valve.Update(collector.Valves[i]);
            }
            ValveAdjustable.Update(collector.ValveAdjustable, showSettings);
            Flowmeter.Update(collector.Flowmeter, showSettings);

            for (int i = 0; i < nValves - 1; i++)
            {
                ValveNums[i] = collector.Loop.ValveNums[i];
                
                int valveInd = ValveNums[i] - 1;
                if (valveInd != -1)
                {
                    DosedVolumes[valveInd] = collector.Loop.DosedVolumes[i];
                    RequiredVolumes[valveInd] = collector.Loop.RequiredVolumes[i];
                }
            }

            if (!IsRatioVolume0Focused)
                RatioVolume0 = collector.Loop.RatioVolume0 ?? 0;

            if (!IsRatioVolume1Focused)
                RatioVolume1 = collector.Loop.RatioVolume1 ?? 0;
            if (!IsRatioVolume2Focused)
                RatioVolume2 = collector.Loop.RatioVolume2 ?? 0;

            if (!IsVolume1Focused)
                Volume1 = collector.Loop.Volume1 ?? 0;
            if (!IsVolume2Focused)
                Volume2 = collector.Loop.Volume2 ?? 0;

            if (!IsSetpoint1Focused)
                Setpoint1 = collector.Loop.Setpoint1 ?? 0;
            if (!IsSetpoint2Focused)
                Setpoint2 = collector.Loop.Setpoint2 ?? 0;
        }

        public void Update(ushort[] registers)
        {
            float setPoint = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VADJ_SP));
            float position = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VADJ_POS));
            ValveAdjustable.Update(setPoint, position);

            float flow = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.FLOW));
            float volume = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VOL));
            float pulsesPerLiter = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VOL_RATIO));
            Flowmeter.Update(flow, volume, pulsesPerLiter);

            DosedVolumes[0] = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VLV_1_DOSE_VOL));
            DosedVolumes[1] = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VLV_2_DOSE_VOL));
            DosedVolumes[2] = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VLV_3_DOSE_VOL));

            RequiredVolumes[0] = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VLV_1_REQ_VOL));
            RequiredVolumes[1] = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VLV_2_REQ_VOL));
            RequiredVolumes[2] = CollectorModbus.Record32.Value(Modbus.Utils.ConcatUshorts(registers, (int)CollectorModbus.Register32.VLV_3_REQ_VOL));
        }

        public int GetValvesNum()
        {
            return nValves;
        }

        //public void InitNew(Collector collector, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(collector.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(collector.Flowmeter, showSettings);
        //}


        // aux values
        public bool IsRatioVolume0Focused
        {
            get { return isRatioVolume0Focused; }
            set { SetProperty(ref isRatioVolume0Focused, value); }
        }

        public bool IsRatioVolume1Focused
        {
            get { return isRatioVolume1Focused; }
            set { SetProperty(ref isRatioVolume1Focused, value); }
        }

        public bool IsRatioVolume2Focused
        {
            get { return isRatioVolume2Focused; }
            set { SetProperty(ref isRatioVolume2Focused, value); }
        }

        public bool IsVolume1Focused
        {
            get { return isVolume1Focused; }
            set { SetProperty(ref isVolume1Focused, value); }
        }

        public bool IsVolume2Focused
        {
            get { return isVolume2Focused; }
            set { SetProperty(ref isVolume2Focused, value); }
        }

        public bool IsSetpoint1Focused
        {
            get { return isSetpoint1Focused; }
            set { SetProperty(ref isSetpoint1Focused, value); }
        }

        public bool IsSetpoint2Focused
        {
            get { return isSetpoint2Focused; }
            set { SetProperty(ref isSetpoint2Focused, value); }
        }

        // read values
        public double RatioVolume0
        {
            get { return ratioVolume0; }
            set { SetProperty(ref ratioVolume0, value); }
        }

        public double RatioVolume1
        {
            get { return ratioVolume1; }
            set { SetProperty(ref ratioVolume1, value); }
        }

        public double RatioVolume2
        {
            get { return ratioVolume2; }
            set { SetProperty(ref ratioVolume2, value); }
        }

        public double Volume1
        {
            get { return volume1; }
            set { SetProperty(ref volume1, value); }
        }

        public double Volume2
        {
            get { return volume2; }
            set { SetProperty(ref volume2, value); }
        }

        public double Setpoint1
        {
            get { return setpoint1; }
            set { SetProperty(ref setpoint1, value); }
        }

        public double Setpoint2
        {
            get { return setpoint2; }
            set { SetProperty(ref setpoint2, value); }
        }
    }
}
