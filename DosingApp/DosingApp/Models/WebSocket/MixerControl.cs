using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DosingApp.Models.WebSocket
{
    public class MixerControl
    {
        //public Mixer Mixer { get; set; }

        public virtual Collector Collector { get; set; }
        public virtual ValveAdjustable ValveAdjustable { get; set; }
        public virtual Flowmeter Flowmeter { get; set; }
        //public ObservableCollection<DispenserCollector> DispenserCollectors { get; set; }
        //public ObservableCollection<DispenserSingle> DispenserSingles { get; set; }
        //public ObservableCollection<DispenserThreeWay> DispenserThreeWays { get; set; }

    }
}
