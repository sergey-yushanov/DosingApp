using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;

namespace DosingApp.Models.Screen
{
    public class SensorScreen : BaseViewModel
    {
        private float raw;
        private float rawLowLimit;
        private float rawHighLimit;
        private float valueLowLimit;
        private float valueHighLimit;

        private bool isRawLowLimitSelected;
        private bool isRawHighLimitSelected;
        private bool isValueLowLimitSelected;
        private bool isValueHighLimitSelected;

        //private float? rawLowLimitNew;
        //private float? rawHighLimitNew;
        //private float? valueLowLimitNew;
        //private float? valueHighLimitNew;

        public void Setup(Sensor sensor)
        {
            Raw = sensor.Raw ?? 0;
            if (!IsRawLowLimitSelected)
                RawLowLimit = sensor.RawLowLimit ?? 0;
            if (!IsRawHighLimitSelected)
                RawHighLimit = sensor.RawHighLimit ?? 0;
            if (!IsValueLowLimitSelected)
                ValueLowLimit = sensor.ValueLowLimit ?? 0;
            if (!IsValueHighLimitSelected)
                ValueHighLimit = sensor.ValueHighLimit ?? 0;
        }

        //public void InitNew(Sensor sensor)
        //{
        //    RawLowLimitNew = sensor.RawLowLimit;
        //    RawHighLimitNew = sensor.RawHighLimit;
        //    ValueLowLimitNew = sensor.ValueLowLimit;
        //    ValueHighLimitNew = sensor.ValueHighLimit;
        //}

        // aux values
        public bool IsRawLowLimitSelected
        {
            get { return isRawLowLimitSelected; }
            set { SetProperty(ref isRawLowLimitSelected, value); }
        }
        
        public bool IsRawHighLimitSelected
        {
            get { return isRawHighLimitSelected; }
            set { SetProperty(ref isRawHighLimitSelected, value); }
        }

        public bool IsValueLowLimitSelected
        {
            get { return isValueLowLimitSelected; }
            set { SetProperty(ref isValueLowLimitSelected, value); }
        }

        public bool IsValueHighLimitSelected
        {
            get { return isValueHighLimitSelected; }
            set { SetProperty(ref isValueHighLimitSelected, value); }
        }

        // read values
        public float Raw
        {
            get { return raw; }
            set { SetProperty(ref raw, value); }
        }

        public float RawLowLimit
        {
            get { return rawLowLimit; }
            set { SetProperty(ref rawLowLimit, value); }
        }

        public float RawHighLimit
        {
            get { return rawHighLimit; }
            set { SetProperty(ref rawHighLimit, value); }
        }

        public float ValueLowLimit
        {
            get { return valueLowLimit; }
            set { SetProperty(ref valueLowLimit, value); }
        }

        public float ValueHighLimit
        {
            get { return valueHighLimit; }
            set { SetProperty(ref valueHighLimit, value); }
        }

        // new values
        //public float? RawLowLimitNew
        //{
        //    get { return rawLowLimitNew; }
        //    set { SetProperty(ref rawLowLimitNew, value); }
        //}

        //public float? RawHighLimitNew
        //{
        //    get { return rawHighLimitNew; }
        //    set { SetProperty(ref rawHighLimitNew, value); }
        //}

        //public float? ValueLowLimitNew
        //{
        //    get { return valueLowLimitNew; }
        //    set { SetProperty(ref valueLowLimitNew, value); }
        //}

        //public float? ValueHighLimitNew
        //{
        //    get { return valueHighLimitNew; }
        //    set { SetProperty(ref valueHighLimitNew, value); }
        //}
    }
}
