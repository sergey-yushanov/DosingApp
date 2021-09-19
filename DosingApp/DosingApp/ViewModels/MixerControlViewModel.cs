using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using DosingApp.Models;
using DosingApp.Models.WebSocket;
using DosingApp.Models.Screen;
using System.Windows.Input;
using Websocket.Client;
using DosingApp.DataContext;
using System.Collections.Generic;
using DosingApp.Views;
using DosingApp.Services;

namespace DosingApp.ViewModels
{
    public class MixerControlViewModel : BaseViewModel
    {
        #region Attributes
        //private IWebsocketClient client;
        //private IncomingMessage incomingMessage;
        //private OutgoingMessage outgoingMessage;
        //private String incomingMessageText;
        //private String outgoingMessageText;
        //private bool isConnected;

        //private bool isFirstMessage;

        //private Mixer mixer;

        public WebSocketService WebSocketService { get; protected set; }

        private string title;

        // mixer parts
        //private MixerControl mixerControl;

        //private List<CollectorScreen> collectors;

        //private CommonScreen common;
        //private CollectorScreen collector;

        //int collectorNumber;
        //private ObservableCollection<ValveScreen> collectorValves;
        //private ValveScreen selectedValve;
        //private ValveAdjustableScreen collectorValveAdjustable;
        //private Flowmeter collectorFlowmeter;

        //private ValveAdjustableScreen valveAdjustable;
        //private Flowmeter flowmeter;

        // commands
        //bool showSettings;

        //public ICommand SendMessageCommand { get; protected set; }

        public ICommand PumpStartCommand { get; protected set; }
        public ICommand PumpStopCommand { get; protected set; }

        public ICommand AckCommand { get; protected set; }

        public ICommand CollectorValveOpenCommand { get; protected set; }
        public ICommand CollectorValveCloseCommand { get; protected set; }

        public ICommand CollectorValveAdjustableOpenCommand { get; protected set; }
        public ICommand CollectorValveAdjustableCloseCommand { get; protected set; }
        public ICommand CollectorValveAdjustableSetpointCommand { get; protected set; }
        public ICommand CollectorValveAdjustableOvertimeCommand { get; protected set; }
        public ICommand CollectorValveAdjustableLimitCloseCommand { get; protected set; }
        public ICommand CollectorValveAdjustableLimitOpenCommand { get; protected set; }
        public ICommand CollectorValveAdjustableDeadbandCloseCommand { get; protected set; }
        public ICommand CollectorValveAdjustableDeadbandOpenCommand { get; protected set; }
        public ICommand CollectorValveAdjustableDeadbandPositionCommand { get; protected set; }
        public ICommand CollectorValveAdjustableCostCloseCommand { get; protected set; }
        public ICommand CollectorValveAdjustableCostOpenCommand { get; protected set; }

        public ICommand CollectorValveAdjustableSensorRawLowLimitCommand { get; protected set; }
        public ICommand CollectorValveAdjustableSensorRawHighLimitCommand { get; protected set; }
        public ICommand CollectorValveAdjustableSensorValueLowLimitCommand { get; protected set; }
        public ICommand CollectorValveAdjustableSensorValueHighimitCommand { get; protected set; }

        public ICommand CollectorFlowmeterNullifyCommand { get; protected set; }
        public ICommand CollectorFlowmeterPulsesPerLiterCommand { get; protected set; }

        public ICommand ValveAdjustableOpenCommand { get; protected set; }
        public ICommand ValveAdjustableCloseCommand { get; protected set; }
        public ICommand ValveAdjustableSetpointCommand { get; protected set; }
        public ICommand ValveAdjustableOvertimeCommand { get; protected set; }
        public ICommand ValveAdjustableLimitCloseCommand { get; protected set; }
        public ICommand ValveAdjustableLimitOpenCommand { get; protected set; }
        public ICommand ValveAdjustableDeadbandCloseCommand { get; protected set; }
        public ICommand ValveAdjustableDeadbandOpenCommand { get; protected set; }
        public ICommand ValveAdjustableDeadbandPositionCommand { get; protected set; }
        public ICommand ValveAdjustableCostCloseCommand { get; protected set; }
        public ICommand ValveAdjustableCostOpenCommand { get; protected set; }

        public ICommand ValveAdjustableSensorRawLowLimitCommand { get; protected set; }
        public ICommand ValveAdjustableSensorRawHighLimitCommand { get; protected set; }
        public ICommand ValveAdjustableSensorValueLowLimitCommand { get; protected set; }
        public ICommand ValveAdjustableSensorValueHighimitCommand { get; protected set; }

        public ICommand FlowmeterNullifyCommand { get; protected set; }
        public ICommand FlowmeterPulsesPerLiterCommand { get; protected set; }

        public ICommand SingleDosValveAdjustableOpenCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableCloseCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableSetpointCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableOvertimeCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableLimitCloseCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableLimitOpenCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableDeadbandCloseCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableDeadbandOpenCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableDeadbandPositionCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableCostCloseCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableCostOpenCommand { get; protected set; }

        public ICommand SingleDosValveAdjustableSensorRawLowLimitCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableSensorRawHighLimitCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableSensorValueLowLimitCommand { get; protected set; }
        public ICommand SingleDosValveAdjustableSensorValueHighimitCommand { get; protected set; }

        public ICommand SingleDosFlowmeterNullifyCommand { get; protected set; }
        public ICommand SingleDosFlowmeterPulsesPerLiterCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public MixerControlViewModel()
        {
            WebSocketService = new WebSocketService();

            // screen
            //GetActiveMixer();

            //if (Mixer != null)
            if (WebSocketService.Mixer != null)
            {
                Title = (WebSocketService.Mixer != null) ? WebSocketService.Mixer.Name + " (Ручной режим)" : "Не выбрана активная установка";

                // create template for mixer control
                //CreateMixerControl(Mixer);

                // websocket
                //isFirstMessage = true;
                //showSettings = true;
                //SendMessageCommand = new Command(SendSettingsMessage);

                //ClientCreate();
                //ConnectToServerAsync();

                PumpStartCommand = new Command(PumpStart);
                PumpStopCommand = new Command(PumpStop);

                AckCommand = new Command(Ack);

                CollectorValveOpenCommand = new Command(CollectorValveOpen);
                CollectorValveCloseCommand = new Command(CollectorValveClose);

                CollectorValveAdjustableOpenCommand = new Command(CollectorValveAdjustableOpen);
                CollectorValveAdjustableCloseCommand = new Command(CollectorValveAdjustableClose);
                CollectorValveAdjustableSetpointCommand = new Command(CollectorValveAdjustableSetpoint);
                CollectorValveAdjustableOvertimeCommand = new Command(CollectorValveAdjustableOvertime);
                CollectorValveAdjustableLimitCloseCommand = new Command(CollectorValveAdjustableLimitClose);
                CollectorValveAdjustableLimitOpenCommand = new Command(CollectorValveAdjustableLimitOpen);
                CollectorValveAdjustableDeadbandCloseCommand = new Command(CollectorValveAdjustableDeadbandClose);
                CollectorValveAdjustableDeadbandOpenCommand = new Command(CollectorValveAdjustableDeadbandOpen);
                CollectorValveAdjustableDeadbandPositionCommand = new Command(CollectorValveAdjustableDeadbandPosition);
                CollectorValveAdjustableCostCloseCommand = new Command(CollectorValveAdjustableCostClose);
                CollectorValveAdjustableCostOpenCommand = new Command(CollectorValveAdjustableCostOpen);

                CollectorValveAdjustableSensorRawLowLimitCommand = new Command(CollectorValveAdjustableSensorRawLowLimit);
                CollectorValveAdjustableSensorRawHighLimitCommand = new Command(CollectorValveAdjustableSensorRawHighLimit);
                CollectorValveAdjustableSensorValueLowLimitCommand = new Command(CollectorValveAdjustableSensorValueLowLimit);
                CollectorValveAdjustableSensorValueHighimitCommand = new Command(CollectorValveAdjustableSensorValueHighimit);

                CollectorFlowmeterNullifyCommand = new Command(CollectorFlowmeterNullify);
                CollectorFlowmeterPulsesPerLiterCommand = new Command(CollectorFlowmeterPulsesPerLiter);

                ValveAdjustableOpenCommand = new Command(ValveAdjustableOpen);
                ValveAdjustableCloseCommand = new Command(ValveAdjustableClose);
                ValveAdjustableSetpointCommand = new Command(ValveAdjustableSetpoint);
                ValveAdjustableOvertimeCommand = new Command(ValveAdjustableOvertime);
                ValveAdjustableLimitCloseCommand = new Command(ValveAdjustableLimitClose);
                ValveAdjustableLimitOpenCommand = new Command(ValveAdjustableLimitOpen);
                ValveAdjustableDeadbandCloseCommand = new Command(ValveAdjustableDeadbandClose);
                ValveAdjustableDeadbandOpenCommand = new Command(ValveAdjustableDeadbandOpen);
                ValveAdjustableDeadbandPositionCommand = new Command(ValveAdjustableDeadbandPosition);
                ValveAdjustableCostCloseCommand = new Command(ValveAdjustableCostClose);
                ValveAdjustableCostOpenCommand = new Command(ValveAdjustableCostOpen);

                ValveAdjustableSensorRawLowLimitCommand = new Command(ValveAdjustableSensorRawLowLimit);
                ValveAdjustableSensorRawHighLimitCommand = new Command(ValveAdjustableSensorRawHighLimit);
                ValveAdjustableSensorValueLowLimitCommand = new Command(ValveAdjustableSensorValueLowLimit);
                ValveAdjustableSensorValueHighimitCommand = new Command(ValveAdjustableSensorValueHighimit);

                FlowmeterNullifyCommand = new Command(FlowmeterNullify);
                FlowmeterPulsesPerLiterCommand = new Command(FlowmeterPulsesPerLiter);


                SingleDosValveAdjustableOpenCommand = new Command(SingleDosValveAdjustableOpen);
                SingleDosValveAdjustableCloseCommand = new Command(SingleDosValveAdjustableClose);
                SingleDosValveAdjustableSetpointCommand = new Command(SingleDosValveAdjustableSetpoint);
                SingleDosValveAdjustableOvertimeCommand = new Command(SingleDosValveAdjustableOvertime);
                SingleDosValveAdjustableLimitCloseCommand = new Command(SingleDosValveAdjustableLimitClose);
                SingleDosValveAdjustableLimitOpenCommand = new Command(SingleDosValveAdjustableLimitOpen);
                SingleDosValveAdjustableDeadbandCloseCommand = new Command(SingleDosValveAdjustableDeadbandClose);
                SingleDosValveAdjustableDeadbandOpenCommand = new Command(SingleDosValveAdjustableDeadbandOpen);
                SingleDosValveAdjustableDeadbandPositionCommand = new Command(SingleDosValveAdjustableDeadbandPosition);
                SingleDosValveAdjustableCostCloseCommand = new Command(SingleDosValveAdjustableCostClose);
                SingleDosValveAdjustableCostOpenCommand = new Command(SingleDosValveAdjustableCostOpen);

                SingleDosValveAdjustableSensorRawLowLimitCommand = new Command(SingleDosValveAdjustableSensorRawLowLimit);
                SingleDosValveAdjustableSensorRawHighLimitCommand = new Command(SingleDosValveAdjustableSensorRawHighLimit);
                SingleDosValveAdjustableSensorValueLowLimitCommand = new Command(SingleDosValveAdjustableSensorValueLowLimit);
                SingleDosValveAdjustableSensorValueHighimitCommand = new Command(SingleDosValveAdjustableSensorValueHighimit);

                SingleDosFlowmeterNullifyCommand = new Command(SingleDosFlowmeterNullify);
                SingleDosFlowmeterPulsesPerLiterCommand = new Command(SingleDosFlowmeterPulsesPerLiter);
            }
        }
        #endregion Constructor

        #region Properties
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        //public WebSocketService WebSocketService
        //{
        //    get { return webSocketService; }
        //    set { SetProperty(ref webSocketService, value); }
        //}

        public CollectorScreen Collector
        {
            get { return WebSocketService.Collector; }
//            set { SetProperty(ref WebSocketService.Collector, value); }
        }

        public CommonScreen Common
        {
            get { return WebSocketService.Common; }
//            set { SetProperty(ref common, value); }
        }

        public SingleDosScreen SingleDos
        {
            get { return WebSocketService.SingleDos; }
            //            set { SetProperty(ref common, value); }
        }

        //public int CollectorNumber
        //{
        //    get { return collectorNumber; }
        //    set { SetProperty(ref collectorNumber, value); }
        //}

        //public ObservableCollection<ValveScreen> CollectorValves
        //{
        //    get { return collectorValves; }
        //    set { SetProperty(ref collectorValves, value); }
        //}

        //public ValveScreen SelectedValve
        //{
        //    get { return selectedValve; }
        //    set { SetProperty(ref selectedValve, value); }
        //}

        //public ValveAdjustableScreen CollectorValveAdjustable
        //{
        //    get { return collectorValveAdjustable; }
        //    set { SetProperty(ref collectorValveAdjustable, value); }
        //}

        //public Flowmeter CollectorFlowmeter
        //{
        //    get { return collectorFlowmeter; }
        //    set { SetProperty(ref collectorFlowmeter, value); }
        //}

        //public ValveAdjustableScreen ValveAdjustable
        //{
        //    get { return valveAdjustable; }
        //    set { SetProperty(ref valveAdjustable, value); }
        //}

        //public Flowmeter Flowmeter
        //{
        //    get { return flowmeter; }
        //    set { SetProperty(ref flowmeter, value); }
        //}

        //public string MixerName
        //{
        //    get { return mixerName; }
        //    set { SetProperty(ref mixerName, value); }
        //}

        //public Mixer Mixer
        //{
        //    get
        //    {
        //        //MixerName = (mixer != null) ? mixer.Name : "Не выбрана активная установка";
        //        return mixer;
        //    }
        //    set { SetProperty(ref mixer, value); }
        //}

        //public IncomingMessage IncomingMessage
        //{
        //    get { return incomingMessage; }
        //    set { SetProperty(ref incomingMessage, value); }
        //}

        //public OutgoingMessage OutgoingMessage
        //{
        //    get { return outgoingMessage; }
        //    set { SetProperty(ref outgoingMessage, value); }
        //}

        //public String IncomingMessageText
        //{
        //    get { return incomingMessageText; }
        //    set { SetProperty(ref incomingMessageText, value); }
        //}

        //public String OutgoingMessageText
        //{
        //    get { return outgoingMessageText; }
        //    set { SetProperty(ref outgoingMessageText, value); }
        //}

        //public bool IsConnected
        //{
        //    get { return WebSocketService.IsConnected; }
        //    //set { SetProperty(ref isConnected, value); }
        //}
        #endregion Properties

        #region Commands
        private void PumpStart()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                PumpStart = true
            };
            Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void PumpStop()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                PumpStop = true
            };
            Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void Ack()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Ack = true
            };
            Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void ShowSettings()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                ShowSettings = true
            };
            Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void HideSettings()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                ShowSettings = false
            };
            Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void CollectorValveOpen(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            WebSocketService.CollectorValveMessage(1, new Valve { Number = valveScreen.Number, CommandOpen = true });
        }

        private void CollectorValveClose(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            WebSocketService.CollectorValveMessage(1, new Valve { Number = valveScreen.Number, CommandClose = true });
        }

        private void CollectorValveAdjustableOpen(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { CommandOpen = true });
        }

        private void CollectorValveAdjustableClose(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { CommandClose = true });
        }

        private void CollectorValveAdjustableSetpoint(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Setpoint = (float)valveAdjustableScreen.Setpoint });
        }

        private void CollectorValveAdjustableOvertime(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Overtime = (int)valveAdjustableScreen.Overtime });
        }

        private void CollectorValveAdjustableLimitClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { LimitClose = (float)valveAdjustableScreen.LimitClose });
        }

        private void CollectorValveAdjustableLimitOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { LimitOpen = (float)valveAdjustableScreen.LimitOpen });
        }

        private void CollectorValveAdjustableDeadbandClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { DeadbandClose = (float)valveAdjustableScreen.DeadbandClose });
        }

        private void CollectorValveAdjustableDeadbandOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { DeadbandOpen = (float)valveAdjustableScreen.DeadbandOpen });
        }

        private void CollectorValveAdjustableDeadbandPosition(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { DeadbandPosition = (float)valveAdjustableScreen.DeadbandPosition });
        }

        private void CollectorValveAdjustableCostClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { CostClose = (float)valveAdjustableScreen.CostClose });
        }

        private void CollectorValveAdjustableCostOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { CostOpen = (float)valveAdjustableScreen.CostOpen });
        }

        private void CollectorValveAdjustableSensorRawLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { RawLowLimit = (float)sensorScreen.RawLowLimit } });
        }

        private void CollectorValveAdjustableSensorRawHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { RawHighLimit = (float)sensorScreen.RawHighLimit } });
        }

        private void CollectorValveAdjustableSensorValueLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { ValueLowLimit = (float)sensorScreen.ValueLowLimit } });
        }

        private void CollectorValveAdjustableSensorValueHighimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { ValueHighLimit = (float)sensorScreen.ValueHighLimit } });
        }

        private void CollectorFlowmeterNullify(object flowmeterInstance)
        {
            //FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            WebSocketService.CollectorFlowmeterMessage(1, new Flowmeter { NullifyVolume = true });
        }

        private void CollectorFlowmeterPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            WebSocketService.CollectorFlowmeterMessage(1, new Flowmeter { PulsesPerLiter = (float)flowmeterScreen.PulsesPerLiter });
        }

        private void ValveAdjustableOpen(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { CommandOpen = true });
        }

        private void ValveAdjustableClose(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { CommandClose = true });
        }

        private void ValveAdjustableSetpoint(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Setpoint = valveAdjustableScreen.Setpoint });
        }

        private void ValveAdjustableOvertime(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Overtime = (int)valveAdjustableScreen.Overtime });
        }

        private void ValveAdjustableLimitClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { LimitClose = (float)valveAdjustableScreen.LimitClose });
        }

        private void ValveAdjustableLimitOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { LimitOpen = (float)valveAdjustableScreen.LimitOpen });
        }

        private void ValveAdjustableDeadbandClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { DeadbandClose = (float)valveAdjustableScreen.DeadbandClose });
        }

        private void ValveAdjustableDeadbandOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { DeadbandOpen = (float)valveAdjustableScreen.DeadbandOpen });
        }

        private void ValveAdjustableDeadbandPosition(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { DeadbandPosition = (float)valveAdjustableScreen.DeadbandPosition });
        }

        private void ValveAdjustableCostClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { CostClose = (float)valveAdjustableScreen.CostClose });
        }

        private void ValveAdjustableCostOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { CostOpen = (float)valveAdjustableScreen.CostOpen });
        }

        private void ValveAdjustableSensorRawLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Sensor = new Sensor { RawLowLimit = (float)sensorScreen.RawLowLimit } });
        }

        private void ValveAdjustableSensorRawHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Sensor = new Sensor { RawHighLimit = (float)sensorScreen.RawHighLimit } });
        }

        private void ValveAdjustableSensorValueLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Sensor = new Sensor { ValueLowLimit = (float)sensorScreen.ValueLowLimit } });
        }

        private void ValveAdjustableSensorValueHighimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Sensor = new Sensor { ValueHighLimit = (float)sensorScreen.ValueHighLimit } });
        }

        private void FlowmeterNullify(object flowmeterInstance)
        {
            //FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            WebSocketService.FlowmeterMessage(new Flowmeter { NullifyVolume = true });
        }

        private void FlowmeterPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            WebSocketService.FlowmeterMessage(new Flowmeter { PulsesPerLiter = (float)flowmeterScreen.PulsesPerLiter });
        }


        private void SingleDosValveAdjustableOpen(object valveAdjustableInstance)
        {
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { CommandOpen = true });
        }

        private void SingleDosValveAdjustableClose(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { CommandClose = true });
        }

        private void SingleDosValveAdjustableSetpoint(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Setpoint = (float)valveAdjustableScreen.Setpoint });
        }

        private void SingleDosValveAdjustableOvertime(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Overtime = (int)valveAdjustableScreen.Overtime });
        }

        private void SingleDosValveAdjustableLimitClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { LimitClose = (float)valveAdjustableScreen.LimitClose });
        }

        private void SingleDosValveAdjustableLimitOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { LimitOpen = (float)valveAdjustableScreen.LimitOpen });
        }

        private void SingleDosValveAdjustableDeadbandClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { DeadbandClose = (float)valveAdjustableScreen.DeadbandClose });
        }

        private void SingleDosValveAdjustableDeadbandOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { DeadbandOpen = (float)valveAdjustableScreen.DeadbandOpen });
        }

        private void SingleDosValveAdjustableDeadbandPosition(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { DeadbandPosition = (float)valveAdjustableScreen.DeadbandPosition });
        }

        private void SingleDosValveAdjustableCostClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { CostClose = (float)valveAdjustableScreen.CostClose });
        }

        private void SingleDosValveAdjustableCostOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { CostOpen = (float)valveAdjustableScreen.CostOpen });
        }

        private void SingleDosValveAdjustableSensorRawLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { RawLowLimit = (float)sensorScreen.RawLowLimit } });
        }

        private void SingleDosValveAdjustableSensorRawHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { RawHighLimit = (float)sensorScreen.RawHighLimit } });
        }

        private void SingleDosValveAdjustableSensorValueLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { ValueLowLimit = (float)sensorScreen.ValueLowLimit } });
        }

        private void SingleDosValveAdjustableSensorValueHighimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { ValueHighLimit = (float)sensorScreen.ValueHighLimit } });
        }

        private void SingleDosFlowmeterNullify(object flowmeterInstance)
        {
            //FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            WebSocketService.SingleFlowmeterMessage(1, new Flowmeter { NullifyVolume = true });
        }

        private void SingleDosFlowmeterPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            WebSocketService.SingleFlowmeterMessage(1, new Flowmeter { PulsesPerLiter = (float)flowmeterScreen.PulsesPerLiter });
        }
        #endregion Commands

        #region Methods
        //void ClientCreate()
        //{
        //    var factory = new Func<ClientWebSocket>(() => new ClientWebSocket
        //    {
        //        Options =
        //        {
        //            KeepAliveInterval = TimeSpan.FromSeconds(5)
        //        }
        //    });
        //    var url = new Uri("ws://192.168.11.1/ws");
        //    client = new WebsocketClient(url, factory);
        //}

        //async void ConnectToServerAsync()
        //{
        //    await Task.Factory.StartNew(async () =>
        //    {
        //        client.ReconnectTimeout = TimeSpan.FromSeconds(30);
        //        client
        //            .ReconnectionHappened
        //            .Subscribe(info =>
        //            {
        //                Console.WriteLine($"Websocket Reconnection happened, type: {info.Type}");
        //                UpdateClientState();
        //            });

        //        client
        //            .DisconnectionHappened
        //            .Subscribe(x =>
        //            {
        //                UpdateClientState();
        //            });

        //        client
        //            .MessageReceived
        //            .Subscribe(message =>
        //            {
        //                UpdateClientState();
        //                IncomingMessageText = message.Text;
        //                IncomingMessage = JsonConvert.DeserializeObject<IncomingMessage>(IncomingMessageText);
        //                UpdateIncomingData();
        //            });

        //        Console.WriteLine("Websocket Starting...");
        //        await client.Start();
        //        Console.WriteLine("Websocket Started.");

        //    }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        //}

        //void UpdateClientState()
        //{
        //    IsConnected = client.IsRunning;
        //}

        //void UpdateIncomingData()
        //{
        //    Collector.Update(IncomingMessage.Collectors[0], IncomingMessage.ShowSettings ?? false);
        //    Common.Update(IncomingMessage.Common, IncomingMessage.ShowSettings ?? false);


        //    //for (int i = 0; i < 4; i++)
        //    //{
        //    //    var valve = Collector.Valves.FirstOrDefault(v => v.Number == IncomingMessage.Collectors[0].Valves[i].Number);
        //    //    valve.Command = (bool)IncomingMessage.Collectors[0].Valves[i].Command;
        //    //}

        //    //Collector.ValveAdjustable.Position = (float)IncomingMessage.Collectors[0].ValveAdjustable.Position;
        //    //Collector.ValveAdjustable.Setpoint = (float)IncomingMessage.Collectors[0].ValveAdjustable.Setpoint;

        //    //if (isFirstMessage)
        //    //{
        //    //    Collector.InitNew(IncomingMessage.Collectors[0], IncomingMessage.ShowSettings ?? false);
        //    //    Common.InitNew(IncomingMessage.Common, IncomingMessage.ShowSettings ?? false);
        //    //    isFirstMessage = false;
        //    //}
        //}

        //private async Task SendMessageAsync(OutgoingMessage outgoingMessage)
        //{
        //    OutgoingMessageText = JsonConvert.SerializeObject(outgoingMessage, Formatting.Indented, new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Ignore
        //    });
        //    Console.WriteLine(OutgoingMessageText);
        //    client.Send(OutgoingMessageText);

        //    OutgoingMessageText = string.Empty;
        //}

        //void SendSettingsMessage()
        //{
        //    showSettings = !showSettings;
        //    var outgoingMessage = new OutgoingMessage
        //    {
        //        ShowSettings = showSettings
        //    };

        //    Task.Run(async () => await SendMessageAsync(outgoingMessage));
        //}

        //public void WebsocketClientExit()
        //{
        //    if (client != null)
        //    {
        //        client.Dispose();
        //        client = null;
        //    }
        //    //client.Dispose();
        //    Console.WriteLine("Websocket Stoped.");
        //}

        //public void GetActiveMixer()
        //{
        //    using (AppDbContext db = App.GetContext())
        //    {
        //        Mixer = db.Mixers.FirstOrDefault(m => m.IsUsedMixer == true);
        //    }
        //}

        //public void CreateMixerControl(Mixer mixer)
        //{
        //    // todo: для других типов дозаторов сделать аналогично
        //    //Collectors = new List<CollectorScreen>((int)mixer.Collector);
        //    //for (int i = 0; i < (int)mixer.Collector; i++)
        //    //{
        //    //    Collectors.Add(new CollectorScreen(i+1));
        //    //}
        //    //Console.WriteLine(Collectors[0].Valves.Count);

        //    Collector = new CollectorScreen(1);
        //    Common = new CommonScreen();
        //}

        //public void CollectorValveMessage(int collectorNumber, Valve valve)
        //{
        //    var outgoingMessage = new OutgoingMessage()
        //    {
        //        Collectors = new List<Collector> {new Collector
        //        {
        //            Number = collectorNumber,
        //            Valves = new List<Valve> { valve }
        //        }}
        //    };
        //    Task.Run(async () => await SendMessageAsync(outgoingMessage));
        //}

        //public void CollectorValveAdjustableMessage(int collectorNumber, ValveAdjustable valveAdjustable)
        //{
        //    var outgoingMessage = new OutgoingMessage()
        //    {
        //        Collectors = new List<Collector> {new Collector
        //        {
        //            Number = collectorNumber,
        //            ValveAdjustable = valveAdjustable
        //        }}
        //    };
        //    Task.Run(async () => await SendMessageAsync(outgoingMessage));
        //}

        //public void CollectorFlowmeterMessage(int collectorNumber, Flowmeter flowmeter)
        //{
        //    var outgoingMessage = new OutgoingMessage()
        //    {
        //        Collectors = new List<Collector> {new Collector
        //        {
        //            Number = collectorNumber,
        //            Flowmeter = flowmeter
        //        }}
        //    };
        //    Task.Run(async () => await SendMessageAsync(outgoingMessage));
        //}

        //public void ValveAdjustableMessage(ValveAdjustable valveAdjustable)
        //{
        //    var outgoingMessage = new OutgoingMessage()
        //    {
        //        Common = new Common
        //        {
        //            ValveAdjustable = valveAdjustable
        //        }
        //    };
        //    Task.Run(async () => await SendMessageAsync(outgoingMessage));
        //}

        //public void FlowmeterMessage(Flowmeter flowmeter)
        //{
        //    var outgoingMessage = new OutgoingMessage()
        //    {
        //        Common = new Common
        //        {
        //            Flowmeter = flowmeter
        //        }
        //    };
        //    Task.Run(async () => await SendMessageAsync(outgoingMessage));
        //}
        #endregion Methods
    }
}
