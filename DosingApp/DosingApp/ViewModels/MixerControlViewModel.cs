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
using DosingApp.Models.Modbus;

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

        //public WebSocketService WebSocketService { get; protected set; }
        public ModbusService ModbusService { get; protected set; }

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
        public ICommand CollectorValveAdjustableSensorValueHighLimitCommand { get; protected set; }


        public ICommand CollectorRatioVolume0Command { get; protected set; }
        public ICommand CollectorRatioVolume1Command { get; protected set; }
        public ICommand CollectorRatioVolume2Command { get; protected set; }

        public ICommand CollectorVolume1Command { get; protected set; }
        public ICommand CollectorVolume2Command { get; protected set; }

        public ICommand CollectorSetpoint1Command { get; protected set; }
        public ICommand CollectorSetpoint2Command { get; protected set; }


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
        public ICommand ValveAdjustableSensorValueHighLimitCommand { get; protected set; }

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
        public ICommand SingleDosValveAdjustableSensorValueHighLimitCommand { get; protected set; }

        public ICommand SingleDosFlowmeterNullifyCommand { get; protected set; }
        public ICommand SingleDosFlowmeterPulsesPerLiterCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public MixerControlViewModel()
        {
            //WebSocketService = new WebSocketService();
            ModbusService = new ModbusService();

            // screen
            //GetActiveMixer();

            //if (Mixer != null)
            //if (WebSocketService.Mixer != null)
            if (ModbusService.Mixer != null)
            {
                //Title = (WebSocketService.Mixer != null) ? WebSocketService.Mixer.Name + " (Ручной режим)" : "Не выбрана активная установка";
                Title = (ModbusService.Mixer != null) ?  ModbusService.Mixer.Name + " (Ручной режим)" : "Не выбрана активная установка";

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
                CollectorValveAdjustableSensorValueHighLimitCommand = new Command(CollectorValveAdjustableSensorValueHighLimit);

                CollectorRatioVolume0Command = new Command(CollectorRatioVolume0);
                CollectorRatioVolume1Command = new Command(CollectorRatioVolume1);
                CollectorRatioVolume2Command = new Command(CollectorRatioVolume2);

                CollectorVolume1Command = new Command(CollectorVolume1);
                CollectorVolume2Command = new Command(CollectorVolume2);

                CollectorSetpoint1Command = new Command(CollectorSetpoint1);
                CollectorSetpoint2Command = new Command(CollectorSetpoint2);

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
                ValveAdjustableSensorValueHighLimitCommand = new Command(ValveAdjustableSensorValueHighLimit);

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
                SingleDosValveAdjustableSensorValueHighLimitCommand = new Command(SingleDosValveAdjustableSensorValueHighLimit);

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
            //get { return WebSocketService.Collector; }
            get { return ModbusService.Collector; }
//            set { SetProperty(ref WebSocketService.Collector, value); }
        }

        public CommonScreen Common
        {
            //get { return WebSocketService.Common; }
            get { return  ModbusService.Common; }
//            set { SetProperty(ref common, value); }
        }

        public SingleDosScreen SingleDos
        {
            //get { return WebSocketService.SingleDos; }
            get { return  ModbusService.SingleDos; }
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

            //var outgoingMessage = new OutgoingMessage()
            //{
            //    PumpStart = true
            //};
            //Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void PumpStop()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                PumpStop = true
            };
            //Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void Ack()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Ack = true
            };
            //Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void ShowSettings()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                ShowSettings = true
            };
            //Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void HideSettings()
        {
            var outgoingMessage = new OutgoingMessage()
            {
                ShowSettings = false
            };
            //Task.Run(async () => await WebSocketService.SendMessageAsync(outgoingMessage));
        }

        private void CollectorValveOpen(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            //WebSocketService.CollectorValveMessage(1, new Valve { Number = valveScreen.Number, CommandOpen = true });
        }

        private void CollectorValveClose(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            //WebSocketService.CollectorValveMessage(1, new Valve { Number = valveScreen.Number, CommandClose = true });
        }

        private void CollectorValveAdjustableOpen(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { CommandOpen = true });
            ModbusService.WriteSingleRegister(ColMod.ValveAdjustableOpen(1));
        }

        private void CollectorValveAdjustableClose(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { CommandClose = true });
            ModbusService.WriteSingleRegister(ColMod.ValveAdjustableClose(1));
        }

        private void CollectorValveAdjustableSetpoint(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Setpoint = (double)valveAdjustableScreen.Setpoint });
            ModbusService.WriteSingleRegister32(ColMod.ValveAdjustableSetpoint(1, (float)valveAdjustableScreen.Setpoint));
        }

        private void CollectorValveAdjustableOvertime(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Overtime = (int)valveAdjustableScreen.Overtime });
        }

        private void CollectorValveAdjustableLimitClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { LimitClose = (double)valveAdjustableScreen.LimitClose });
        }

        private void CollectorValveAdjustableLimitOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { LimitOpen = (double)valveAdjustableScreen.LimitOpen });
        }

        private void CollectorValveAdjustableDeadbandClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { DeadbandClose = (double)valveAdjustableScreen.DeadbandClose });
        }

        private void CollectorValveAdjustableDeadbandOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { DeadbandOpen = (double)valveAdjustableScreen.DeadbandOpen });
        }

        private void CollectorValveAdjustableDeadbandPosition(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { DeadbandPosition = (double)valveAdjustableScreen.DeadbandPosition });
        }

        private void CollectorValveAdjustableCostClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { CostClose = (double)valveAdjustableScreen.CostClose });
        }

        private void CollectorValveAdjustableCostOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { CostOpen = (double)valveAdjustableScreen.CostOpen });
        }

        private void CollectorValveAdjustableSensorRawLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { RawLowLimit = (double)sensorScreen.RawLowLimit } });
        }

        private void CollectorValveAdjustableSensorRawHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { RawHighLimit = (double)sensorScreen.RawHighLimit } });
        }

        private void CollectorValveAdjustableSensorValueLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { ValueLowLimit = (double)sensorScreen.ValueLowLimit } });
        }

        private void CollectorValveAdjustableSensorValueHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.CollectorValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { ValueHighLimit = (double)sensorScreen.ValueHighLimit } });
        }

        private void CollectorRatioVolume0(object value)
        {
            //WebSocketService.CollectorLoopMessage(1, new CollectorLoop { RatioVolume0 = (double)value });
        }

        private void CollectorRatioVolume1(object value)
        {
            //WebSocketService.CollectorLoopMessage(1, new CollectorLoop { RatioVolume1 = (double)value });
        }

        private void CollectorRatioVolume2(object value)
        {
            //WebSocketService.CollectorLoopMessage(1, new CollectorLoop { RatioVolume2 = (double)value });
        }

        private void CollectorVolume1(object value)
        {
            //WebSocketService.CollectorLoopMessage(1, new CollectorLoop { Volume1 = (double)value });
        }

        private void CollectorVolume2(object value)
        {
            //WebSocketService.CollectorLoopMessage(1, new CollectorLoop { Volume2 = (double)value });
        }

        private void CollectorSetpoint1(object value)
        {
            //WebSocketService.CollectorLoopMessage(1, new CollectorLoop { Setpoint1 = (double)value });
        }

        private void CollectorSetpoint2(object value)
        {
            //WebSocketService.CollectorLoopMessage(1, new CollectorLoop { Setpoint2 = (double)value });
        }

        private void CollectorFlowmeterNullify(object flowmeterInstance)
        {
            //FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            //WebSocketService.CollectorFlowmeterMessage(1, new Flowmeter { NullifyVolume = true });
        }

        private void CollectorFlowmeterPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            //WebSocketService.CollectorFlowmeterMessage(1, new Flowmeter { PulsesPerLiter = (double)flowmeterScreen.PulsesPerLiter });
        }

        private void ValveAdjustableOpen(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { CommandOpen = true });
        }

        private void ValveAdjustableClose(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { CommandClose = true });
        }

        private void ValveAdjustableSetpoint(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Setpoint = valveAdjustableScreen.Setpoint });
        }

        private void ValveAdjustableOvertime(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Overtime = (int)valveAdjustableScreen.Overtime });
        }

        private void ValveAdjustableLimitClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { LimitClose = (double)valveAdjustableScreen.LimitClose });
        }

        private void ValveAdjustableLimitOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { LimitOpen = (double)valveAdjustableScreen.LimitOpen });
        }

        private void ValveAdjustableDeadbandClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { DeadbandClose = (double)valveAdjustableScreen.DeadbandClose });
        }

        private void ValveAdjustableDeadbandOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { DeadbandOpen = (double)valveAdjustableScreen.DeadbandOpen });
        }

        private void ValveAdjustableDeadbandPosition(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { DeadbandPosition = (double)valveAdjustableScreen.DeadbandPosition });
        }

        private void ValveAdjustableCostClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { CostClose = (double)valveAdjustableScreen.CostClose });
        }

        private void ValveAdjustableCostOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { CostOpen = (double)valveAdjustableScreen.CostOpen });
        }

        private void ValveAdjustableSensorRawLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Sensor = new Sensor { RawLowLimit = (double)sensorScreen.RawLowLimit } });
        }

        private void ValveAdjustableSensorRawHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Sensor = new Sensor { RawHighLimit = (double)sensorScreen.RawHighLimit } });
        }

        private void ValveAdjustableSensorValueLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Sensor = new Sensor { ValueLowLimit = (double)sensorScreen.ValueLowLimit } });
        }

        private void ValveAdjustableSensorValueHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.ValveAdjustableMessage(new ValveAdjustable { Sensor = new Sensor { ValueHighLimit = (double)sensorScreen.ValueHighLimit } });
        }

        private void FlowmeterNullify(object flowmeterInstance)
        {
            //FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            //WebSocketService.FlowmeterMessage(new Flowmeter { NullifyVolume = true });
        }

        private void FlowmeterPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            //WebSocketService.FlowmeterMessage(new Flowmeter { PulsesPerLiter = (double)flowmeterScreen.PulsesPerLiter });
        }


        private void SingleDosValveAdjustableOpen(object valveAdjustableInstance)
        {
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { CommandOpen = true });
        }

        private void SingleDosValveAdjustableClose(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { CommandClose = true });
        }

        private void SingleDosValveAdjustableSetpoint(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Setpoint = (double)valveAdjustableScreen.Setpoint });
        }

        private void SingleDosValveAdjustableOvertime(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Overtime = (int)valveAdjustableScreen.Overtime });
        }

        private void SingleDosValveAdjustableLimitClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { LimitClose = (double)valveAdjustableScreen.LimitClose });
        }

        private void SingleDosValveAdjustableLimitOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { LimitOpen = (double)valveAdjustableScreen.LimitOpen });
        }

        private void SingleDosValveAdjustableDeadbandClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { DeadbandClose = (double)valveAdjustableScreen.DeadbandClose });
        }

        private void SingleDosValveAdjustableDeadbandOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { DeadbandOpen = (double)valveAdjustableScreen.DeadbandOpen });
        }

        private void SingleDosValveAdjustableDeadbandPosition(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { DeadbandPosition = (double)valveAdjustableScreen.DeadbandPosition });
        }

        private void SingleDosValveAdjustableCostClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { CostClose = (double)valveAdjustableScreen.CostClose });
        }

        private void SingleDosValveAdjustableCostOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { CostOpen = (double)valveAdjustableScreen.CostOpen });
        }

        private void SingleDosValveAdjustableSensorRawLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { RawLowLimit = (double)sensorScreen.RawLowLimit } });
        }

        private void SingleDosValveAdjustableSensorRawHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { RawHighLimit = (double)sensorScreen.RawHighLimit } });
        }

        private void SingleDosValveAdjustableSensorValueLowLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { ValueLowLimit = (double)sensorScreen.ValueLowLimit } });
        }

        private void SingleDosValveAdjustableSensorValueHighLimit(object valveAdjustableSensorInstance)
        {
            SensorScreen sensorScreen = valveAdjustableSensorInstance as SensorScreen;
            //WebSocketService.SingleValveAdjustableMessage(1, new ValveAdjustable { Sensor = new Sensor { ValueHighLimit = (double)sensorScreen.ValueHighLimit } });
        }

        private void SingleDosFlowmeterNullify(object flowmeterInstance)
        {
            //FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            //WebSocketService.SingleFlowmeterMessage(1, new Flowmeter { NullifyVolume = true });
        }

        private void SingleDosFlowmeterPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            //WebSocketService.SingleFlowmeterMessage(1, new Flowmeter { PulsesPerLiter = (double)flowmeterScreen.PulsesPerLiter });
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

        //    //Collector.ValveAdjustable.Position = (double)IncomingMessage.Collectors[0].ValveAdjustable.Position;
        //    //Collector.ValveAdjustable.Setpoint = (double)IncomingMessage.Collectors[0].ValveAdjustable.Setpoint;

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
