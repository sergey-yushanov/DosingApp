using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System;

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

        private double ratioVolume;
        private double ratioVolumeMicro;
        private double microVolume;
        private double microSetpoint;

        private bool isRatioVolumeFocused;
        private bool isRatioVolumeMicroFocused;
        private bool isMicroVolumeFocused;
        private bool isMicroSetpointFocused;

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

            if (!IsRatioVolumeFocused)
                RatioVolume = collector.Loop.RatioVolume ?? 0;
            if (!IsRatioVolumeMicroFocused)
                RatioVolumeMicro = collector.Loop.RatioVolumeMicro ?? 0;
            if (!IsMicroVolumeFocused)
                MicroVolume = collector.Loop.MicroVolume ?? 0;
            if (!IsMicroSetpointFocused)
                MicroSetpoint = collector.Loop.MicroSetpoint ?? 0;
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
        public bool IsRatioVolumeFocused
        {
            get { return isRatioVolumeFocused; }
            set { SetProperty(ref isRatioVolumeFocused, value); }
        }

        public bool IsRatioVolumeMicroFocused
        {
            get { return isRatioVolumeMicroFocused; }
            set { SetProperty(ref isRatioVolumeMicroFocused, value); }
        }

        public bool IsMicroVolumeFocused
        {
            get { return isMicroVolumeFocused; }
            set { SetProperty(ref isMicroVolumeFocused, value); }
        }

        public bool IsMicroSetpointFocused
        {
            get { return isMicroSetpointFocused; }
            set { SetProperty(ref isMicroSetpointFocused, value); }
        }

        // read values
        public double RatioVolume
        {
            get { return ratioVolume; }
            set { SetProperty(ref ratioVolume, value); }
        }

        public double RatioVolumeMicro
        {
            get { return ratioVolumeMicro; }
            set { SetProperty(ref ratioVolumeMicro, value); }
        }

        public double MicroVolume
        {
            get { return microVolume; }
            set { SetProperty(ref microVolume, value); }
        }

        public double MicroSetpoint
        {
            get { return microSetpoint; }
            set { SetProperty(ref microSetpoint, value); }
        }
    }
}
