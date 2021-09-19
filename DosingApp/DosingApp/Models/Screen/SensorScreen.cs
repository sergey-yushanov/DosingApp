using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;

namespace DosingApp.Models.Screen
{
    public class SensorScreen : BaseViewModel
    {
        private double raw;
        private double rawLowLimit;
        private double rawHighLimit;
        private double valueLowLimit;
        private double valueHighLimit;

        private bool isRawLowLimitFocused;
        private bool isRawHighLimitFocused;
        private bool isValueLowLimitFocused;
        private bool isValueHighLimitFocused;

        //private double? rawLowLimitNew;
        //private double? rawHighLimitNew;
        //private double? valueLowLimitNew;
        //private double? valueHighLimitNew;

        public void Setup(Sensor sensor)
        {
            Raw = sensor.Raw ?? 0;
            if (!IsRawLowLimitFocused)
                RawLowLimit = sensor.RawLowLimit ?? 0;
            if (!IsRawHighLimitFocused)
                RawHighLimit = sensor.RawHighLimit ?? 0;
            if (!IsValueLowLimitFocused)
                ValueLowLimit = sensor.ValueLowLimit ?? 0;
            if (!IsValueHighLimitFocused)
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
        public bool IsRawLowLimitFocused
        {
            get { return isRawLowLimitFocused; }
            set { SetProperty(ref isRawLowLimitFocused, value); }
        }
        
        public bool IsRawHighLimitFocused
        {
            get { return isRawHighLimitFocused; }
            set { SetProperty(ref isRawHighLimitFocused, value); }
        }

        public bool IsValueLowLimitFocused
        {
            get { return isValueLowLimitFocused; }
            set { SetProperty(ref isValueLowLimitFocused, value); }
        }

        public bool IsValueHighLimitFocused
        {
            get { return isValueHighLimitFocused; }
            set { SetProperty(ref isValueHighLimitFocused, value); }
        }

        // read values
        public double Raw
        {
            get { return raw; }
            set { SetProperty(ref raw, value); }
        }

        public double RawLowLimit
        {
            get { return rawLowLimit; }
            set { SetProperty(ref rawLowLimit, value); }
        }

        public double RawHighLimit
        {
            get { return rawHighLimit; }
            set { SetProperty(ref rawHighLimit, value); }
        }

        public double ValueLowLimit
        {
            get { return valueLowLimit; }
            set { SetProperty(ref valueLowLimit, value); }
        }

        public double ValueHighLimit
        {
            get { return valueHighLimit; }
            set { SetProperty(ref valueHighLimit, value); }
        }

        // new values
        //public double? RawLowLimitNew
        //{
        //    get { return rawLowLimitNew; }
        //    set { SetProperty(ref rawLowLimitNew, value); }
        //}

        //public double? RawHighLimitNew
        //{
        //    get { return rawHighLimitNew; }
        //    set { SetProperty(ref rawHighLimitNew, value); }
        //}

        //public double? ValueLowLimitNew
        //{
        //    get { return valueLowLimitNew; }
        //    set { SetProperty(ref valueLowLimitNew, value); }
        //}

        //public double? ValueHighLimitNew
        //{
        //    get { return valueHighLimitNew; }
        //    set { SetProperty(ref valueHighLimitNew, value); }
        //}
    }
}
