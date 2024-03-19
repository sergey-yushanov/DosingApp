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
    public class ManualControlViewModel : BaseViewModel
    {
        #region Attributes
        public ModbusService ModbusService { get; protected set; }
        private string title;
        private bool isContentVisible;
        private bool isCollector1Visible;
        private bool isCollector2Visible;
        private bool isCollector3Visible;
        private bool isCollector4Visible;
        private bool isVolumeDosVisible;
        private bool isPowderDosVisible;

        // commands
        public ICommand AckCommand { get; protected set; }

        public ICommand PumpStartCommand { get; protected set; }
        public ICommand PumpStopCommand { get; protected set; }

        public ICommand CarrierValveOpenCommand { get; protected set; }
        public ICommand CarrierValveCloseCommand { get; protected set; }

        public ICommand Collector1ValveAdjustableOpenCommand { get; protected set; }
        public ICommand Collector1ValveAdjustableCloseCommand { get; protected set; }

        public ICommand Collector1ValveOpenCommand { get; protected set; }
        public ICommand Collector1ValveCloseCommand { get; protected set; }

        public ICommand Collector2ValveAdjustableOpenCommand { get; protected set; }
        public ICommand Collector2ValveAdjustableCloseCommand { get; protected set; }

        public ICommand Collector2ValveOpenCommand { get; protected set; }
        public ICommand Collector2ValveCloseCommand { get; protected set; }

        public ICommand Collector3ValveAdjustableOpenCommand { get; protected set; }
        public ICommand Collector3ValveAdjustableCloseCommand { get; protected set; }

        public ICommand Collector3ValveOpenCommand { get; protected set; }
        public ICommand Collector3ValveCloseCommand { get; protected set; }

        public ICommand Collector4ValveAdjustableOpenCommand { get; protected set; }
        public ICommand Collector4ValveAdjustableCloseCommand { get; protected set; }

        public ICommand Collector4ValveOpenCommand { get; protected set; }
        public ICommand Collector4ValveCloseCommand { get; protected set; }

        public ICommand VolumeDosValveOpenCommand { get; protected set; }
        public ICommand VolumeDosValveCloseCommand { get; protected set; }

        public ICommand PowderDosValveOpenCommand { get; protected set; }
        public ICommand PowderDosValveCloseCommand { get; protected set; }

        public ICommand CarrierResetCommand { get; protected set; }
        public ICommand Collector1ResetCommand { get; protected set; }
        public ICommand Collector2ResetCommand { get; protected set; }
        public ICommand Collector3ResetCommand { get; protected set; }
        public ICommand Collector4ResetCommand { get; protected set; }
        public ICommand VolumeDosResetCommand { get; protected set; }
        public ICommand PowderDosResetCommand { get; protected set; }

        private string carrierInfo;
        private string collector1Info;
        private string collector1FlowInfo;
        private string collector2Info;
        private string collector2FlowInfo;
        private string collector3Info;
        private string collector3FlowInfo;
        private string collector4Info;
        private string collector4FlowInfo;
        private string volumeDosInfo;
        private string powderDosInfo;
        #endregion Attributes

        #region Constructor
        public ManualControlViewModel()
        {
            ModbusService = new ModbusService();

            if (ModbusService.Mixer != null)
            {
                PumpStartCommand = new Command(PumpStart);
                PumpStopCommand = new Command(PumpStop);

                AckCommand = new Command(Ack);

                CarrierValveOpenCommand = new Command(CarrierValveOpen);
                CarrierValveCloseCommand = new Command(CarrierValveClose);

                Collector1ValveAdjustableOpenCommand = new Command(Collector1ValveAdjustableOpen);
                Collector1ValveAdjustableCloseCommand = new Command(Collector1ValveAdjustableClose);

                Collector1ValveOpenCommand = new Command(Collector1ValveOpen);
                Collector1ValveCloseCommand = new Command(Collector1ValveClose);

                Collector2ValveAdjustableOpenCommand = new Command(Collector2ValveAdjustableOpen);
                Collector2ValveAdjustableCloseCommand = new Command(Collector2ValveAdjustableClose);

                Collector2ValveOpenCommand = new Command(Collector2ValveOpen);
                Collector2ValveCloseCommand = new Command(Collector2ValveClose);

                Collector3ValveAdjustableOpenCommand = new Command(Collector3ValveAdjustableOpen);
                Collector3ValveAdjustableCloseCommand = new Command(Collector3ValveAdjustableClose);

                Collector3ValveOpenCommand = new Command(Collector3ValveOpen);
                Collector3ValveCloseCommand = new Command(Collector3ValveClose);

                Collector4ValveAdjustableOpenCommand = new Command(Collector4ValveAdjustableOpen);
                Collector4ValveAdjustableCloseCommand = new Command(Collector4ValveAdjustableClose);

                Collector4ValveOpenCommand = new Command(Collector4ValveOpen);
                Collector4ValveCloseCommand = new Command(Collector4ValveClose);

                VolumeDosValveOpenCommand = new Command(VolumeDosValveOpen);
                VolumeDosValveCloseCommand = new Command(VolumeDosValveClose);

                PowderDosValveOpenCommand = new Command(PowderDosValveOpen);
                PowderDosValveCloseCommand = new Command(PowderDosValveClose);

                CarrierResetCommand = new Command(CarrierReset);
                Collector1ResetCommand = new Command(Collector1Reset);
                Collector2ResetCommand = new Command(Collector2Reset);
                Collector3ResetCommand = new Command(Collector3Reset);
                Collector4ResetCommand = new Command(Collector4Reset);
                VolumeDosResetCommand = new Command(VolumeDosReset);
                PowderDosResetCommand = new Command(PowderDosReset);

                IsCollector1Visible = (ModbusService.Mixer.Collector == 1) || (ModbusService.Mixer.Collector == 2);
                IsCollector2Visible = ModbusService.Mixer.Collector == 2;
                IsCollector3Visible = (ModbusService.Mixer.Collector == 3) || (ModbusService.Mixer.Collector == 4);
                IsCollector4Visible = ModbusService.Mixer.Collector == 4;
                IsVolumeDosVisible = ModbusService.Mixer.Volume == 1;
                IsPowderDosVisible = ModbusService.Mixer.Powder == 1;
            }

            Title = (ModbusService.Mixer != null) ? "Ручное управление" : "Не задана активная установка";

            IsContentVisible = ModbusService.IsConnected;
        }
        #endregion Constructor

        #region Properties
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool IsContentVisible
        {
            get { return isContentVisible; }
            set { SetProperty(ref isContentVisible, value); }
        }

        public bool IsCollector1Visible
        {
            get { return isCollector1Visible; }
            set { SetProperty(ref isCollector1Visible, value); }
        }

        public bool IsCollector2Visible
        {
            get { return isCollector2Visible; }
            set { SetProperty(ref isCollector2Visible, value); }
        }

        public bool IsCollector3Visible
        {
            get { return isCollector3Visible; }
            set { SetProperty(ref isCollector3Visible, value); }
        }

        public bool IsCollector4Visible
        {
            get { return isCollector4Visible; }
            set { SetProperty(ref isCollector4Visible, value); }
        }

        public bool IsVolumeDosVisible
        {
            get { return isVolumeDosVisible; }
            set { SetProperty(ref isVolumeDosVisible, value); }
        }

        public bool IsPowderDosVisible
        {
            get { return isPowderDosVisible; }
            set { SetProperty(ref isPowderDosVisible, value); }
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
            get { return ModbusService.VolumeDos; }
        }

        public PowderDosScreen PowderDos
        {
            get { return ModbusService.PowderDos; }
        }


        public string CarrierInfo
        {
            get { return carrierInfo; }
            set { SetProperty(ref carrierInfo, value); }
        }

        public string Collector1Info
        {
            get { return collector1Info; }
            set { SetProperty(ref collector1Info, value); }
        }

        public string Collector2Info
        {
            get { return collector2Info; }
            set { SetProperty(ref collector2Info, value); }
        }

        public string Collector3Info
        {
            get { return collector3Info; }
            set { SetProperty(ref collector3Info, value); }
        }

        public string Collector4Info
        {
            get { return collector4Info; }
            set { SetProperty(ref collector4Info, value); }
        }

        public string Collector1FlowInfo
        {
            get { return collector1FlowInfo; }
            set { SetProperty(ref collector1FlowInfo, value); }
        }

        public string Collector2FlowInfo
        {
            get { return collector2FlowInfo; }
            set { SetProperty(ref collector2FlowInfo, value); }
        }

        public string Collector3FlowInfo
        {
            get { return collector3FlowInfo; }
            set { SetProperty(ref collector3FlowInfo, value); }
        }

        public string Collector4FlowInfo
        {
            get { return collector4FlowInfo; }
            set { SetProperty(ref collector4FlowInfo, value); }
        }

        public string VolumeDosInfo
        {
            get { return volumeDosInfo; }
            set { SetProperty(ref volumeDosInfo, value); }
        }

        public string PowderDosInfo
        {
            get { return powderDosInfo; }
            set { SetProperty(ref powderDosInfo, value); }
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

        //public bool IsConnected
        //{
        //    get { return WebSocketService.IsConnected; }
        //    //set { SetProperty(ref isConnected, value); }
        //}
        #endregion Properties

        #region Commands
        private void Ack()
        {
            ModbusService.WriteSingleRegister(CommonModbus.Ack());
        }

        private void PumpStart()
        {
            ModbusService.WriteSingleRegister(CommonModbus.PumpStart());
        }

        private void PumpStop()
        {
            ModbusService.WriteSingleRegister(CommonModbus.PumpStop());
        }

        private void CarrierValveOpen()
        {
            ModbusService.WriteSingleRegister(CommonModbus.ValveOpen());
        }

        private void CarrierValveClose()
        {
            ModbusService.WriteSingleRegister(CommonModbus.ValveClose());
        }

        private void Collector1ValveAdjustableOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveAdjustableOpen(1));
        }

        private void Collector1ValveAdjustableClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveAdjustableClose(1));
        }

        private void Collector1ValveOpen(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveOpen(1, (ushort)valveScreen.Number));
        }

        private void Collector1ValveClose(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveClose(1, (ushort)valveScreen.Number));
        }

        private void Collector2ValveAdjustableOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveAdjustableOpen(2));
        }

        private void Collector2ValveAdjustableClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveAdjustableClose(2));
        }

        private void Collector2ValveOpen(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveOpen(2, (ushort)valveScreen.Number));
        }

        private void Collector2ValveClose(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveClose(2, (ushort)valveScreen.Number));
        }

        private void Collector3ValveAdjustableOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveAdjustableOpen(3));
        }

        private void Collector3ValveAdjustableClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveAdjustableClose(3));
        }

        private void Collector3ValveOpen(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveOpen(3, (ushort)valveScreen.Number));
        }

        private void Collector3ValveClose(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveClose(3, (ushort)valveScreen.Number));
        }

        private void Collector4ValveAdjustableOpen(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveAdjustableOpen(4));
        }

        private void Collector4ValveAdjustableClose(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveAdjustableClose(4));
        }

        private void Collector4ValveOpen(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveOpen(4, (ushort)valveScreen.Number));
        }

        private void Collector4ValveClose(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            ModbusService.WriteSingleRegister(CollectorModbus.ValveClose(4, (ushort)valveScreen.Number));
        }

        private void VolumeDosValveOpen()
        {
            ModbusService.WriteSingleRegister(VolumeDosModbus.ValveOpen(1));
        }

        private void VolumeDosValveClose()
        {
            ModbusService.WriteSingleRegister(VolumeDosModbus.ValveClose(1));
        }

        private void PowderDosValveOpen()
        {
            ModbusService.WriteSingleRegister(PowderDosModbus.ValveOpen(1));
        }

        private void PowderDosValveClose()
        {
            ModbusService.WriteSingleRegister(PowderDosModbus.ValveClose(1));
        }

        private void CarrierReset()
        {
            ModbusService.WriteSingleRegister(CommonModbus.VolumeReset());
        }

        private void Collector1Reset()
        {
            ModbusService.WriteSingleRegister(CollectorModbus.VolumeReset(1));
        }

        private void Collector2Reset()
        {
            ModbusService.WriteSingleRegister(CollectorModbus.VolumeReset(2));
        }

        private void Collector3Reset()
        {
            ModbusService.WriteSingleRegister(CollectorModbus.VolumeReset(3));
        }

        private void Collector4Reset()
        {
            ModbusService.WriteSingleRegister(CollectorModbus.VolumeReset(4));
        }

        private void VolumeDosReset()
        {
            ModbusService.WriteSingleRegister(VolumeDosModbus.VolumeReset(1));
        }

        private void PowderDosReset()
        {
            ModbusService.WriteSingleRegister(PowderDosModbus.VolumeReset(1));
        }
        #endregion Commands

        #region Methods
        public void Update()
        {
            ModbusService.MasterMessages();

            CarrierInfo = Common.Flowmeter.Volume.ToString("N2") + " л   ";
            Collector1Info = Collector1.Flowmeter.Volume.ToString("N2") + " л   ";
            Collector1FlowInfo = Collector1.Flowmeter.Flow.ToString("N2") + " л/мин ";
            Collector2Info = Collector2.Flowmeter.Volume.ToString("N2") + " л   ";
            Collector2FlowInfo = Collector2.Flowmeter.Flow.ToString("N2") + " л/мин ";
            Collector3Info = Collector3.Flowmeter.Volume.ToString("N2") + " л   ";
            Collector3FlowInfo = Collector3.Flowmeter.Flow.ToString("N2") + " л/мин ";
            Collector4Info = Collector4.Flowmeter.Volume.ToString("N2") + " л   ";
            Collector4FlowInfo = Collector4.Flowmeter.Flow.ToString("N2") + " л/мин ";
            VolumeDosInfo = VolumeDos.Flowmeter.Volume.ToString("N2") + " л   ";
            PowderDosInfo = PowderDos.Flowmeter.Volume.ToString("N2") + " л   ";
        }
        #endregion Methods
    }
}
