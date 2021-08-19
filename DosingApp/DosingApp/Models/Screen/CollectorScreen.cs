using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace DosingApp.Models.Screen
{
    public class CollectorScreen : BaseViewModel
    {
        private const int nValves = 4;  // в коллекторе 4 клапана on/off

        public int Number { get; set; }
        public ObservableCollection<ValveScreen> Valves { get; set; }
        public ValveAdjustableScreen ValveAdjustable { get; set; }
        public FlowmeterScreen Flowmeter { get; set; }
        public ObservableCollection<float> RequiredVolumes { get; set; }
        public ObservableCollection<float> DosedVolumes { get; set; }

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
            
            RequiredVolumes = new ObservableCollection<float>();
            DosedVolumes = new ObservableCollection<float>();
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

            //for (int i = 0; i < nValves-1; i++)
            //{
            //    DosedVolumes[i] = collector.Loop.DosedVolumes[i];
            //    RequiredVolumes[i] = collector.Loop.RequiredVolumes[i];
            //}
        }

        //public void InitNew(Collector collector, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(collector.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(collector.Flowmeter, showSettings);
        //}
    }
}
