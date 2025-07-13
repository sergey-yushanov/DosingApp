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

        public ICommand CarrierPulsesPerLiterCommand { get; protected set; }
        public ICommand Collector1PulsesPerLiterCommand { get; protected set; }
        public ICommand Collector2PulsesPerLiterCommand { get; protected set; }
        public ICommand Collector3PulsesPerLiterCommand { get; protected set; }
        public ICommand Collector4PulsesPerLiterCommand { get; protected set; }
        public ICommand VolumeDosPulsesPerLiterCommand { get; protected set; }
        public ICommand PowderDosPulsesPerLiterCommand { get; protected set; }

        public ICommand CollectorFineK11Command { get; protected set; }
        public ICommand CollectorFineK12Command { get; protected set; }
        public ICommand CollectorFineK13Command { get; protected set; }
        public ICommand CollectorFineSetPoint1Command { get; protected set; }

        public ICommand CollectorFineK21Command { get; protected set; }
        public ICommand CollectorFineK22Command { get; protected set; }
        public ICommand CollectorFineK23Command { get; protected set; }
        public ICommand CollectorFineSetPoint2Command { get; protected set; }

        public ICommand CollectorFineK31Command { get; protected set; }
        public ICommand CollectorFineK32Command { get; protected set; }
        public ICommand CollectorFineK33Command { get; protected set; }
        public ICommand CollectorFineSetPoint3Command { get; protected set; }

        public ICommand CollectorFineVol_1_2Command { get; protected set; }
        public ICommand CollectorFineVol_2_3Command { get; protected set; }

        public ICommand VolumeDosFineK4Command { get; protected set; }
        public ICommand CollectorFillReqVolCommand { get; protected set; }
        public ICommand PowderDosFineK5Command { get; protected set; }

        public ICommand CollectorDryCommand { get; protected set; }
        public ICommand VolumeDosDryCommand { get; protected set; }
        public ICommand PowderDosDryCommand { get; protected set; }

        public ICommand VolumeDosDryEnableCommand { get; protected set; }
        public ICommand VolumeDosDryDisableCommand { get; protected set; }

        public ICommand VolumeDosDelayVolumeCommand { get; protected set; }

        public ICommand PowderDosDryEnableCommand { get; protected set; }
        public ICommand PowderDosDryDisableCommand { get; protected set; }

        public ICommand PowderDosDelayVolumeCommand { get; protected set; }

        public ICommand CarrierDeltaVolumeCommand { get; protected set; }
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
                Collector3PulsesPerLiterCommand = new Command(Collector3PulsesPerLiter);
                Collector4PulsesPerLiterCommand = new Command(Collector4PulsesPerLiter);
                VolumeDosPulsesPerLiterCommand = new Command(VolumeDosPulsesPerLiter);
                PowderDosPulsesPerLiterCommand = new Command(PowderDosPulsesPerLiter);

                CollectorFineK11Command = new Command(CollectorFineK11);
                CollectorFineK12Command = new Command(CollectorFineK12);
                CollectorFineK13Command = new Command(CollectorFineK13);
                CollectorFineSetPoint1Command = new Command(CollectorFineSetPoint1);

                CollectorFineK21Command = new Command(CollectorFineK21);
                CollectorFineK22Command = new Command(CollectorFineK22);
                CollectorFineK23Command = new Command(CollectorFineK23);
                CollectorFineSetPoint2Command = new Command(CollectorFineSetPoint2);

                CollectorFineK31Command = new Command(CollectorFineK31);
                CollectorFineK32Command = new Command(CollectorFineK32);
                CollectorFineK33Command = new Command(CollectorFineK33);
                CollectorFineSetPoint3Command = new Command(CollectorFineSetPoint3);

                CollectorFineVol_1_2Command = new Command(CollectorFineVol_1_2);
                CollectorFineVol_2_3Command = new Command(CollectorFineVol_2_3);

                VolumeDosFineK4Command = new Command(VolumeDosFineK4);
                PowderDosFineK5Command = new Command(PowderDosFineK5);

                CollectorFillReqVolCommand = new Command(CollectorFillReqVol);

                CollectorDryCommand = new Command(CollectorDry);
                VolumeDosDryCommand = new Command(VolumeDosDry);
                PowderDosDryCommand = new Command(PowderDosDry);

                VolumeDosDryEnableCommand = new Command(VolumeDosDryEnable);
                VolumeDosDryDisableCommand = new Command(VolumeDosDryDisable);

                VolumeDosDelayVolumeCommand = new Command(VolumeDosDelayVolume);

                PowderDosDryEnableCommand = new Command(PowderDosDryEnable);
                PowderDosDryDisableCommand = new Command(PowderDosDryDisable);

                PowderDosDelayVolumeCommand = new Command(PowderDosDelayVolume);

                CarrierDeltaVolumeCommand = new Command(CarrierDeltaVolume);
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
            get { return ModbusService.Collectors[0]; }
        }

        public CollectorScreen Collector2
        {
            get { return ModbusService.Collectors[1]; }
        }

        public CollectorScreen Collector3
        {
            get { return ModbusService.Collectors[2]; }
        }

        public CollectorScreen Collector4
        {
            get { return ModbusService.Collectors[3]; }
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
            get { return ModbusService.VolumeDoses[0]; }
        }

        public PowderDosScreen PowderDos
        {
            get { return ModbusService.PowderDoses[0]; }
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

        private void Collector3PulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            ModbusService.WriteSingleRegister32(CollectorModbus.VolumeRatio(3, (float)flowmeterScreen.PulsesPerLiter));
        }

        private void Collector4PulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            ModbusService.WriteSingleRegister32(CollectorModbus.VolumeRatio(4, (float)flowmeterScreen.PulsesPerLiter));
        }

        private void VolumeDosPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            ModbusService.WriteSingleRegister32(VolumeDosModbus.VolumeRatio(1, (float)flowmeterScreen.PulsesPerLiter));
        }

        private void PowderDosPulsesPerLiter(object flowmeterInstance)
        {
            FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            ModbusService.WriteSingleRegister32(PowderDosModbus.VolumeRatio(1, (float)flowmeterScreen.PulsesPerLiter));
        }

        private void CollectorFineK11(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK11((float)commonScreen.CollectorFineK11));
        }

        private void CollectorFineK12(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK12((float)commonScreen.CollectorFineK12));
        }

        private void CollectorFineK13(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK13((float)commonScreen.CollectorFineK13));
        }

        private void CollectorFineSetPoint1(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineSetPoint1((float)commonScreen.CollectorFineSetPoint1));
        }

        private void CollectorFineK21(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK21((float)commonScreen.CollectorFineK21));
        }

        private void CollectorFineK22(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK22((float)commonScreen.CollectorFineK22));
        }

        private void CollectorFineK23(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK23((float)commonScreen.CollectorFineK23));
        }

        private void CollectorFineSetPoint2(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineSetPoint2((float)commonScreen.CollectorFineSetPoint2));
        }

        private void CollectorFineK31(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK31((float)commonScreen.CollectorFineK31));
        }

        private void CollectorFineK32(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK32((float)commonScreen.CollectorFineK32));
        }

        private void CollectorFineK33(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineK33((float)commonScreen.CollectorFineK33));
        }

        private void CollectorFineSetPoint3(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineSetPoint3((float)commonScreen.CollectorFineSetPoint3));
        }

        private void CollectorFineVol_1_2(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineVol_1_2((float)commonScreen.CollectorFineVol_1_2));
        }

        private void CollectorFineVol_2_3(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFineVol_2_3((float)commonScreen.CollectorFineVol_2_3));
        }

        private void VolumeDosFineK4(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.VolumeDosFineK4((float)commonScreen.VolumeDosFineK4));
        }

        private void PowderDosFineK5(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.PowderDosFineK5((float)commonScreen.PowderDosFineK5));
        }

        private void CollectorFillReqVol(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorFillReqVol((float)commonScreen.CollectorFillReqVol));
        }

        private void CollectorDry(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CollectorDry((float)commonScreen.CollectorDry));
        }

        private void VolumeDosDry(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.VolumeDosDry((float)commonScreen.VolumeDosDry));
        }

        private void VolumeDosDryEnable(object commonInstance)
        {
            ModbusService.WriteSingleRegister(VolumeDosModbus.VolumeDosDryEnable());
        }

        private void VolumeDosDryDisable(object commonInstance)
        {
            ModbusService.WriteSingleRegister(VolumeDosModbus.VolumeDosDryDisable());
        }

        private void VolumeDosDelayVolume(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.VolumeDosDelayVolume((float)commonScreen.VolumeDosDelayVolume));
        }

        private void PowderDosDry(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.PowderDosDry((float)commonScreen.PowderDosDry));
        }

        private void PowderDosDryEnable(object commonInstance)
        {
            ModbusService.WriteSingleRegister(PowderDosModbus.PowderDosDryEnable());
        }

        private void PowderDosDryDisable(object commonInstance)
        {
            ModbusService.WriteSingleRegister(PowderDosModbus.PowderDosDryDisable());
        }

        private void PowderDosDelayVolume(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.PowderDosDelayVolume((float)commonScreen.PowderDosDelayVolume));
        }

        private void CarrierDeltaVolume(object commonInstance)
        {
            CommonScreen commonScreen = commonInstance as CommonScreen;
            ModbusService.WriteSingleRegister32(CommonModbus.CarrierDeltaVolume((float)commonScreen.CarrierDeltaVolume));
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
