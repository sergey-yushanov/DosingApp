using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace DosingApp.Models.Screen
{
    public class VolumeDosScreen : BaseViewModel
    {
        public int Number { get; set; }
        public ValveScreen Valve { get; set; }
        public FlowmeterScreen Flowmeter { get; set; }
        
        public double? RequiredVolume { get; set; }
        public double? DosedVolume { get; set; }

        public VolumeDosScreen(int number)
        {
            this.Number = number;
            Valve = new ValveScreen() { Name = this.Number.ToString() + "ОД" };
            Flowmeter = new FlowmeterScreen();

            RequiredVolume = 0;
            DosedVolume = 0;
        }

        public string Name { get { return "Объемный дозатор " + Number.ToString() + " (ОД " + Number.ToString() + ")"; } }

        public void Update(VolumeDos volumeDos, bool showSettings)
        {
            Valve.Update(volumeDos.Valve);
            Flowmeter.Update(volumeDos.Flowmeter, showSettings);

            DosedVolume = volumeDos.Loop.DosedVolume;
            RequiredVolume = volumeDos.Loop.RequiredVolume;
        }

        //public void InitNew(Collector collector, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(collector.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(collector.Flowmeter, showSettings);
        //}
    }
}
