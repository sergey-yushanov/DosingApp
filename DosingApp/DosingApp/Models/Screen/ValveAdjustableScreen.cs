using DosingApp.Models.WebSocket;
using DosingApp.ViewModels;

namespace DosingApp.Models.Screen
{
    public class ValveAdjustableScreen : BaseViewModel
    {
        private bool faulty;
        private double setpoint;
        private double position;
        private int overtime;
        private double limitClose;
        private double limitOpen;
        private double deadbandClose;
        private double deadbandOpen;
        private double deadbandPosition;
        private double costClose;
        private double costOpen;

        private bool isSetpointFocused;
        private bool isOvertimeFocused;
        private bool isLimitCloseFocused;
        private bool isLimitOpenFocused;
        private bool isDeadbandCloseFocused;
        private bool isDeadbandOpenFocused;
        private bool isDeadbandPositionFocused;
        private bool isCostCloseFocused;
        private bool isCostOpenFocused;

        //private double? setpointNew;
        //private double? limitCloseNew;
        //private double? limitOpenNew;
        //private double? deadbandCloseNew;
        //private double? deadbandOpenNew;
        //private double? deadbandPositionNew;
        //private double? costCloseNew;
        //private double? costOpenNew;

        public string Name { get; set; }

        public ValveAdjustableScreen()
        {
            Sensor = new SensorScreen();
        }

        public void Update(ValveAdjustable valveAdjustable, bool ShowSettings)
        {
            Faulty = valveAdjustable.Faulty ?? false;
            if (!IsSetpointFocused)
                Setpoint = valveAdjustable.Setpoint ?? 0;
            Position = valveAdjustable.Position ?? 0;
            if (ShowSettings)
            {
                if (!IsOvertimeFocused)
                    Overtime = valveAdjustable.Overtime ?? 150;
                if (!IsLimitCloseFocused)
                    LimitClose = valveAdjustable.LimitClose ?? 0;
                if (!IsLimitOpenFocused)
                    LimitOpen = valveAdjustable.LimitOpen ?? 100;
                if (!IsDeadbandCloseFocused)
                    DeadbandClose = valveAdjustable.DeadbandClose ?? 1;
                if (!IsDeadbandOpenFocused)
                    DeadbandOpen = valveAdjustable.DeadbandOpen ?? 1;
                if (!IsDeadbandPositionFocused)
                    DeadbandPosition = valveAdjustable.DeadbandPosition ?? 1;
                if (!IsCostCloseFocused)
                    CostClose = valveAdjustable.CostClose ?? 1;
                if (!IsCostOpenFocused)
                    CostOpen = valveAdjustable.CostOpen ?? 1;
                Sensor.Setup(valveAdjustable.Sensor);
            }
        }

        //public void InitNew(ValveAdjustable valveAdjustable, bool ShowSettings)
        //{
        //    SetpointNew = valveAdjustable.Setpoint;
        //    if (ShowSettings)
        //    {
        //        LimitCloseNew = valveAdjustable.LimitClose;
        //        LimitOpenNew = valveAdjustable.LimitOpen;
        //        DeadbandCloseNew = valveAdjustable.DeadbandClose;
        //        DeadbandOpenNew = valveAdjustable.DeadbandOpen;
        //        DeadbandPositionNew = valveAdjustable.DeadbandPosition;
        //        CostCloseNew = valveAdjustable.CostClose;
        //        CostOpenNew = valveAdjustable.CostOpen;
        //        Sensor.InitNew(valveAdjustable.Sensor);
        //    }
        //}

        // aux values
        public bool IsSetpointFocused
        {
            get { return isSetpointFocused; }
            set { SetProperty(ref isSetpointFocused, value); }
        }

        public bool IsOvertimeFocused
        {
            get { return isOvertimeFocused; }
            set { SetProperty(ref isOvertimeFocused, value); }
        }

        public bool IsLimitCloseFocused
        {
            get { return isLimitCloseFocused; }
            set { SetProperty(ref isLimitCloseFocused, value); }
        }

        public bool IsLimitOpenFocused
        {
            get { return isLimitOpenFocused; }
            set { SetProperty(ref isLimitOpenFocused, value); }
        }

        public bool IsDeadbandCloseFocused
        {
            get { return isDeadbandCloseFocused; }
            set { SetProperty(ref isDeadbandCloseFocused, value); }
        }

        public bool IsDeadbandOpenFocused
        {
            get { return isDeadbandOpenFocused; }
            set { SetProperty(ref isDeadbandOpenFocused, value); }
        }

        public bool IsDeadbandPositionFocused
        {
            get { return isDeadbandPositionFocused; }
            set { SetProperty(ref isDeadbandPositionFocused, value); }
        }

        public bool IsCostCloseFocused
        {
            get { return isCostCloseFocused; }
            set { SetProperty(ref isCostCloseFocused, value); }
        }

        public bool IsCostOpenFocused
        {
            get { return isCostOpenFocused; }
            set { SetProperty(ref isCostOpenFocused, value); }
        }
        // read values
        public bool Faulty
        {
            get { return faulty; }
            set { SetProperty(ref faulty, value); }
        }

        public double Setpoint
        {
            get { return setpoint; }
            set { SetProperty(ref setpoint, value); }
        }

        public double Position
        {
            get { return position; }
            set { SetProperty(ref position, value); }
        }

        public int Overtime
        {
            get { return overtime; }
            set { SetProperty(ref overtime, value); }
        }

        public double LimitClose
        {
            get { return limitClose; }
            set { SetProperty(ref limitClose, value); }
        }

        public double LimitOpen
        {
            get { return limitOpen; }
            set { SetProperty(ref limitOpen, value); }
        }

        public double DeadbandClose
        {
            get { return deadbandClose; }
            set { SetProperty(ref deadbandClose, value); }
        }

        public double DeadbandOpen
        {
            get { return deadbandOpen; }
            set { SetProperty(ref deadbandOpen, value); }
        }

        public double DeadbandPosition
        {
            get { return deadbandPosition; }
            set { SetProperty(ref deadbandPosition, value); }
        }

        public double CostClose
        {
            get { return costClose; }
            set { SetProperty(ref costClose, value); }
        }

        public double CostOpen
        {
            get { return costOpen; }
            set { SetProperty(ref costOpen, value); }
        }

        public SensorScreen Sensor { get; set; }

        // new values
        //public double? SetpointNew
        //{
        //    get { return setpointNew; }
        //    set { SetProperty(ref setpointNew, value); }
        //}

        //public double? LimitCloseNew
        //{
        //    get { return limitCloseNew; }
        //    set { SetProperty(ref limitCloseNew, value); }
        //}

        //public double? LimitOpenNew
        //{
        //    get { return limitOpenNew; }
        //    set { SetProperty(ref limitOpenNew, value); }
        //}

        //public double? DeadbandCloseNew
        //{
        //    get { return deadbandCloseNew; }
        //    set { SetProperty(ref deadbandCloseNew, value); }
        //}

        //public double? DeadbandOpenNew
        //{
        //    get { return deadbandOpenNew; }
        //    set { SetProperty(ref deadbandOpenNew, value); }
        //}

        //public double? DeadbandPositionNew
        //{
        //    get { return deadbandPositionNew; }
        //    set { SetProperty(ref deadbandPositionNew, value); }
        //}

        //public double? CostCloseNew
        //{
        //    get { return costCloseNew; }
        //    set { SetProperty(ref costCloseNew, value); }
        //}

        //public double? CostOpenNew
        //{
        //    get { return costOpenNew; }
        //    set { SetProperty(ref costOpenNew, value); }
        //}

    }
}
