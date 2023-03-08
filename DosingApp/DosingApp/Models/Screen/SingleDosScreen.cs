using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace DosingApp.Models.Screen
{
    public class SingleDosScreen : BaseViewModel
    {
        public int Number { get; set; }
        public ValveAdjustableScreen ValveAdjustable { get; set; }
        public FlowmeterScreen Flowmeter { get; set; }
        
        public double? RequiredVolume { get; set; }
        public double? DosedVolume { get; set; }

        public SingleDosScreen(int number)
        {
            this.Number = number;
            ValveAdjustable = new ValveAdjustableScreen() { Name = this.Number.ToString() + "ОД" };
            Flowmeter = new FlowmeterScreen();

            RequiredVolume = 0;
            DosedVolume = 0;
        }

        public string Name { get { return "Одиночный дозатор " + Number.ToString() + " (ОД " + Number.ToString() + ")"; } }

        public void Update(SingleDos singleDos, bool showSettings)
        {
            ValveAdjustable.Update(singleDos.ValveAdjustable, showSettings);
            Flowmeter.Update(singleDos.Flowmeter, showSettings);

            DosedVolume = singleDos.Loop.DosedVolume;
            RequiredVolume = singleDos.Loop.RequiredVolume;
        }

        //public void InitNew(Collector collector, bool showSettings)
        //{
        //    ValveAdjustable.InitNew(collector.ValveAdjustable, showSettings);
        //    Flowmeter.InitNew(collector.Flowmeter, showSettings);
        //}
    }
}
