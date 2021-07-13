using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.Screen
{
    public class CollectorScreen
    {
        public int Number { get; set; }
        public ObservableCollection<ValveScreen> Valves { get; set; }
        public ValveAdjustableScreen ValveAdjustable { get; set; }
        public FlowmeterScreen Flowmeter { get; set; }

        public CollectorScreen(int number)
        {
            this.Number = number;

            // в коллекторе 4 клапана on/off
            Valves = new ObservableCollection<ValveScreen>();
            for (int i = 0; i < 4; i++)
            {
                Valves.Add(new ValveScreen() { Number = i + 1, Name = this.Number.ToString() + "Кл" + (i + 1).ToString() });
            }
            //ValveAdjustable = new ValveAdjustableScreen() { CollectorNumber = this.Number };
            ValveAdjustable = new ValveAdjustableScreen();
            Flowmeter = new FlowmeterScreen();
        }

        public string Name { get { return "Коллективный дозатор (КД) - " + Number.ToString(); } }
    }
}
