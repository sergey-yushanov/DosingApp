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
using NModbus.Utility;
using DosingApp.Models.Modbus;
using DosingApp.Models.WebSocket;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DosingApp.Services
{
    public class ModbusService : BaseViewModel
    {
        #region Attributes
        private const byte slaveId = 1;
        private const string defaultUrl = "192.168.3.5";
        private string url;

        private TcpClient tcpClient;
        private IModbusMaster modbusMaster;
        private bool isConnected;

        private Mixer mixer;
        private CommonScreen common;
        private CollectorScreen collector1;
        private CollectorScreen collector2;
        private SingleDosScreen singleDos;
        private VolumeDosScreen volumeDos;
        #endregion Attributes

        #region Constructor
        public ModbusService()
        {
            IsConnected = false;
            GetActiveMixer();
            if (Mixer != null)
            {
                url = (Mixer.Url != null) ? Mixer.Url: defaultUrl;
                CreateMixerControl(Mixer);
                MasterCreate();
            }
        }

        ~ModbusService()
        {
            MasterDispose();
        }
        #endregion Constructor

        #region Properties
        public Mixer Mixer
        {
            get { return mixer; }
            set { SetProperty(ref mixer, value); }
        }

        public CollectorScreen Collector1
        {
            get { return collector1; }
            set { SetProperty(ref collector1, value); }
        }

        public CollectorScreen Collector2
        {
            get { return collector2; }
            set { SetProperty(ref collector2, value); }
        }

        public SingleDosScreen SingleDos
        {
            get { return singleDos; }
            set { SetProperty(ref singleDos, value); }
        }

        public VolumeDosScreen VolumeDos
        {
            get { return volumeDos; }
            set { SetProperty(ref volumeDos, value); }
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

            Common = new CommonScreen();
            Collector1 = new CollectorScreen(1);
            Collector2 = new CollectorScreen(2);
            VolumeDos = new VolumeDosScreen(1);
        }

        public void MasterCreate()
        {
            tcpClient = new TcpClient();
            MasterConnect();

            var factory = new ModbusFactory();
            modbusMaster = factory.CreateMaster(tcpClient);
        }

        public void MasterConnect()
        {
            UpdateClientState();

            if (IsConnected)
                return;

            try
            {
                if (tcpClient.ConnectAsync(url, 502).Wait(TimeSpan.FromSeconds(2)))
                {
                    Console.WriteLine($"TCP Client connect attempt ok");
                }
                else
                {
                    Console.WriteLine($"TCP Client connect attempt false");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            UpdateClientState();
            Console.WriteLine($"TCP Client connected = {IsConnected}");
        }

        public void MasterDispose()
        {
            if (modbusMaster != null)
            {
                modbusMaster.Dispose();
            }

            if (tcpClient != null)
            {
                tcpClient.Close();
            }
            UpdateClientState();
            Console.WriteLine($"TCP Client connected = {IsConnected}");
        }

        public void MasterMessages()
        {
            MasterConnect();

            if (!IsConnected)
                return;

            Common.Update(ReadRegisters(Registers.Common, CommonModbus.numberOfPoints));
            Collector1.Update(ReadRegisters(Registers.Collectors[0], CollectorModbus.numberOfPoints));
            Collector2.Update(ReadRegisters(Registers.Collectors[1], CollectorModbus.numberOfPoints));
            VolumeDos.Update(ReadRegisters(Registers.VolumeDoses[0], VolumeDosModbus.numberOfPoints));
        }

        private void UpdateClientState()
        {
            if (tcpClient != null)
            {
                IsConnected = (tcpClient.Client != null) ? tcpClient.Connected : false;
            }
            else
            {
                IsConnected = false;
            }
        }

        public void WriteSingleRegister(RegisterValue registerValue)
        {
            if (!IsConnected) return;
            modbusMaster.WriteSingleRegister(slaveId, registerValue.Register, registerValue.Value);
            Console.WriteLine($"{DateTime.Now}\tWriteSingleRegister\tHR_{registerValue.Register} = {registerValue.Value}");
        }

        public void WriteSingleRegister32(RegisterValue32 registerValue)
        {
            if (!IsConnected) return;
            modbusMaster.WriteSingleRegister32(slaveId, registerValue.Register, registerValue.Value);
            Console.WriteLine($"{DateTime.Now}\tWriteSingleRegister32\tHR_{registerValue.Register} = {registerValue.Value}");
        }

        public ushort[] ReadRegisters(ushort startAddress, ushort numberOfPoints)
        {
            try
            {
                return modbusMaster.ReadHoldingRegisters(slaveId, startAddress, numberOfPoints);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ushort[numberOfPoints];
            }
        }
        #endregion Methods
    }
}
