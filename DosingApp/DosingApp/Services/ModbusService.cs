using DosingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using NModbus;
using DosingApp.Models;
using DosingApp.Models.Screen;
using DosingApp.DataContext;
using System.Linq;
using System.Net.Sockets;
using NModbus.Extensions.Enron;

namespace DosingApp.Services
{
    public class ModbusService : BaseViewModel
    {
        #region Attributes
        private TcpClient tcpClient;
        private IModbusMaster modbusMaster;
        private bool isConnected;

        private ushort[] registersCommon;
        private ushort[,] registersCollector;
        private ushort[] registersSingleDos;

        private byte slaveId;
        private ushort numRegistersCommon;
        private ushort numRegistersCollector;
        private ushort numRegistersSingleDos;

        private ushort startAddressCommon;
        private ushort startAddressCollector;
        private ushort startAddressSingleDos;

        //private bool isConnected;

        private Mixer mixer;
        private CommonScreen common;
        private CollectorScreen collector;
        private SingleDosScreen singleDos;

        private ushort counter;
        #endregion Attributes

        #region Constructor
        public ModbusService()
        {
            GetActiveMixer();
            if (Mixer != null)
            {
                CreateMixerControl(Mixer);
                MasterCreate();
            }
            
            slaveId = 1;
            numRegistersCommon = 24;
            numRegistersCollector = 20;
            numRegistersSingleDos = 10;

            startAddressCommon = 16384;
            startAddressCollector = 16384;
            startAddressSingleDos = 16384;

            registersCommon = new ushort[numRegistersCommon];
            counter = 0;
        }
        #endregion Constructor

        #region Properties
        public Mixer Mixer
        {
            get { return mixer; }
            set { SetProperty(ref mixer, value); }
        }

        public CollectorScreen Collector
        {
            get { return collector; }
            set { SetProperty(ref collector, value); }
        }

        public SingleDosScreen SingleDos
        {
            get { return singleDos; }
            set { SetProperty(ref singleDos, value); }
        }

        public CommonScreen Common
        {
            get { return common; }
            set { SetProperty(ref common, value); }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { SetProperty(ref isConnected, value); }
        }

        public ushort TestRegister
        {
            get 
            {
                return registersCommon[numRegistersCommon-1];
            }
        }
        #endregion Properties

        #region Methods
        public void GetActiveMixer()
        {
            using (AppDbContext db = App.GetContext())
            {
                Mixer = db.Mixers.FirstOrDefault(m => m.IsUsedMixer == true);
            }
        }

        public void CreateMixerControl(Mixer mixer)
        {
            // todo: для других типов дозаторов сделать аналогично
            //Collectors = new List<CollectorScreen>((int)mixer.Collector);
            //for (int i = 0; i < (int)mixer.Collector; i++)
            //{
            //    Collectors.Add(new CollectorScreen(i+1));
            //}
            //Console.WriteLine(Collectors[0].Valves.Count);

            Collector = new CollectorScreen(2);
            SingleDos = new SingleDosScreen(1);
            Common = new CommonScreen();
        }

        public void MasterCreate()
        {
            tcpClient = new TcpClient();
            tcpClient.Connect("192.168.1.234", 502);
            UpdateClientState();

            var factory = new ModbusFactory();
            modbusMaster = factory.CreateMaster(tcpClient);
        }

        public void MasterDispose()
        {
            if (modbusMaster != null)
            {
                modbusMaster.Dispose();
                //modbusMaster = null;
            }

            if (tcpClient != null)
            {
                tcpClient.Close();
                //tcpClient = null;
            }
            UpdateClientState();
        }

        public void MasterMessages()
        {
            counter++;
            //UInt32 www = 0x42c80083;
            modbusMaster.WriteSingleRegister(slaveId, startAddressCommon, counter);
            registersCommon = modbusMaster.ReadHoldingRegisters(slaveId, startAddressCommon, numRegistersCommon);
            Console.WriteLine($"{DateTime.Now}\tHR_{(startAddressCommon + numRegistersCommon - 1)} = {registersCommon[numRegistersCommon-1]}");

            //for (int i = 0; i < numRegistersCommon; i++)
            //{
            //    Console.WriteLine($"HoldingRegisters {(startAddressCommon + i)}={registers[i]}");
            //}
        }

        private void UpdateClientState()
        {
            IsConnected = tcpClient.Connected;
            Console.WriteLine($"TCP Client connected = {IsConnected}");
        }

        #endregion Methods
    }
}
