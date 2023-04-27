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
        private bool isConnected;

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

        public ICommand CarrierPulsesPerLiterCommand { get; protected set; }
        public ICommand Collector1PulsesPerLiterCommand { get; protected set; }
        public ICommand Collector2PulsesPerLiterCommand { get; protected set; }
        public ICommand VolumeDosPulsesPerLiterCommand { get; protected set; }

        public ICommand CollectorFineK1Command { get; protected set; }
        public ICommand CollectorFineK2Command { get; protected set; }
        public ICommand CollectorFineK3Command { get; protected set; }
        public ICommand CollectorFineSetPointCommand { get; protected set; }
        public ICommand VolumeDosFineK4Command { get; protected set; }
        #endregion Attributes

        #region Constructor
        public MixerControlViewModel()
        {
            ModbusService = new ModbusService();

            if (ModbusService.Mixer != null)
            {
                Title = (ModbusService.Mixer != null) ?  ModbusService.Mixer.Name + " (Ручной режим)" : "Не выбрана активная установка";

                CarrierPulsesPerLiterCommand = new Command(CarrierPulsesPerLiter);
                Collector1PulsesPerLiterCommand = new Command(Collector1PulsesPerLiter);
                Collector2PulsesPerLiterCommand = new Command(Collector2PulsesPerLiter);
                VolumeDosPulsesPerLiterCommand = new Command(VolumeDosPulsesPerLiter);

                CollectorFineK1Command = new Command(CollectorFineK1);
                CollectorFineK2Command = new Command(CollectorFineK2);
                CollectorFineK3Command = new Command(CollectorFineK3);
                CollectorFineSetPointCommand = new Command(CollectorFineSetPoint);

                VolumeDosFineK4Command = new Command(VolumeDosFineK4);
            }
        }
        #endregion Constructor

        #region Properties
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public CollectorScreen Collector1
        {
            get { return ModbusService.Collector1; }
        }

        public CollectorScreen Collector2
        {
            get { return ModbusService.Collector2; }
        }

        public CommonScreen Common
        {
            get { return  ModbusService.Common; }
        }

        //public SingleDosScreen SingleDos
        //{
        //    get { return  ModbusService.SingleDos; }
        //}

        public VolumeDosScreen VolumeDos
        {
            get { return ModbusService.VolumeDos; }
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
        private void CarrierPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.VolumeRatio((float)flowmeterScreen.PulsesPerLiter));
        }

        private void Collector1PulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            ModbusService.WriteSingleRegister32(CollectorModbus.VolumeRatio(1, (float)flowmeterScreen.PulsesPerLiter));
        }

        private void Collector2PulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            ModbusService.WriteSingleRegister32(CollectorModbus.VolumeRatio(2, (float)flowmeterScreen.PulsesPerLiter));
        }

        private void VolumeDosPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            ModbusService.WriteSingleRegister32(VolumeDosModbus.VolumeRatio(1, (float)flowmeterScreen.PulsesPerLiter));
        }

        private void CollectorFineK1(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK1((float)commonScreen.CollectorFineK1));
        }

        private void CollectorFineK2(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK2((float)commonScreen.CollectorFineK2));
        }

        private void CollectorFineK3(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK3((float)commonScreen.CollectorFineK3));
        }

        private void CollectorFineSetPoint(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineSetPoint((float)commonScreen.CollectorFineSetPoint));
        }

        private void VolumeDosFineK4(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.VolumeDosFineK4((float)commonScreen.VolumeDosFineK4));
        }
        #endregion Commands

        #region Methods
        public void Update()
        {
            ModbusService.MasterMessages();
        }
        #endregion Methods
    }
}
