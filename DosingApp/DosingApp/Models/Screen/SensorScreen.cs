using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp.Models.Screen
{
    public class SensorScreen : BaseViewModel
    {
        private float raw;
        public float Raw
        {
            get { return raw; }
            set { SetProperty(ref raw, value); }
        }

        private float rawLowLimit;
        public float RawLowLimit
        {
            get { return rawLowLimit; }
            set { SetProperty(ref rawLowLimit, value); }
        }

        private float rawHighLimit;
        public float RawHighLimit
        {
            get { return rawHighLimit; }
            set { SetProperty(ref rawHighLimit, value); }
        }

        private float valueLowLimit;
        public float ValueLowLimit
        {
            get { return valueLowLimit; }
            set { SetProperty(ref valueLowLimit, value); }
        }

        private float valueHighLimit;
        public float ValueHighLimit
        {
            get { return valueHighLimit; }
            set { SetProperty(ref valueHighLimit, value); }
        }
    }
}
