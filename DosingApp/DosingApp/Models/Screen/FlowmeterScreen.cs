using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;

namespace DosingApp.Models.Screen
{
    public class FlowmeterScreen : BaseViewModel
    {
        private double flow;
        private double volume;
        private double pulsesPerLiter;

        private bool isPulsesPerLiterFocused;

        //private double? pulsesPerLiterNew;

        public void Update(Flowmeter flowmeter, bool showSettings)
        {
            Flow = flowmeter.Flow ?? 0;
            Volume = flowmeter.Volume ?? 0;

            if (showSettings && !IsPulsesPerLiterFocused)
                PulsesPerLiter = flowmeter.PulsesPerLiter ?? 0;
        }

        public void Update(float flow, float volume, float pulsesPerLiter)
        {
            Flow = flow;
            Volume = volume;
            PulsesPerLiter = pulsesPerLiter;
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
        public double Flow
        {
            get { return flow; }
            set { SetProperty(ref flow, value); }
        }

        public double Volume
        {
            get { return volume; }
            set { SetProperty(ref volume, value); }
        }

        public double PulsesPerLiter
        {
            get { return pulsesPerLiter; }
            set { SetProperty(ref pulsesPerLiter, value); }
        }

        // new values
        //public double? PulsesPerLiterNew
        //{
        //    get { return pulsesPerLiterNew; }
        //    set { SetProperty(ref pulsesPerLiterNew, value); }
        //}
    }
}
