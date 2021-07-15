using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;

namespace DosingApp.Models.Screen
{
    public class FlowmeterScreen : BaseViewModel
    {
        private float flow;
        private float volume;
        private float pulsesPerLiter;

        private bool isPulsesPerLiterFocused;

        //private float? pulsesPerLiterNew;

        public void Update(Flowmeter flowmeter, bool showSettings)
        {
            Flow = flowmeter.Flow ?? 0;
            Volume = flowmeter.Volume ?? 0;

            if (showSettings && !IsPulsesPerLiterFocused)
                PulsesPerLiter = flowmeter.PulsesPerLiter ?? 0;
        }

        //public void InitNew(Flowmeter flowmeter, bool showSettings)
        //{
        //    if (showSettings)
        //        PulsesPerLiterNew = flowmeter.PulsesPerLiter;
        //}

        // aux values
        public bool IsPulsesPerLiterFocused
        {
            get { return isPulsesPerLiterFocused; }
            set { SetProperty(ref isPulsesPerLiterFocused, value); }
        }

        // read values
        public float Flow
        {
            get { return flow; }
            set { SetProperty(ref flow, value); }
        }

        public float Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }

        public float PulsesPerLiter
        {
            get { return pulsesPerLiter; }
            set { SetProperty(ref pulsesPerLiter, value); }
        }

        // new values
        //public float? PulsesPerLiterNew
        //{
        //    get { return pulsesPerLiterNew; }
        //    set { SetProperty(ref pulsesPerLiterNew, value); }
        //}
    }
}
