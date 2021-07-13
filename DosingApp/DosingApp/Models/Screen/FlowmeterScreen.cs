using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.Screen
{
    public class FlowmeterScreen : BaseViewModel
    {
        private float flow;
        public float Flow
        {
            get { return flow; }
            set { SetProperty(ref flow, value); }
        }

        private float volume;
        public float Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }

        private float pulsesPerLiter;
        public float PulsesPerLiter
        {
            get { return pulsesPerLiter; }
            set { SetProperty(ref pulsesPerLiter, value); }
        }
    }
}
