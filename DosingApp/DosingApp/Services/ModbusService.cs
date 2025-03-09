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
        readonly private string url;

        private TcpClient tcpClient;
        private IModbusMaster modbusMaster;
        private bool isConnected;

        private Mixer mixer;
        private CommonScreen common;
        
        private List<CollectorScreen> collectors;

        private List<SingleDosScreen> singleDoses;
        private List<VolumeDosScreen> volumeDoses;
        private List<PowderDosScreen> powderDoses;
        #endregion Attributes

        #region Constructor
        public ModbusService()
        {
            IsConnected = false;
            GetActiveMixer();
            if (Mixer != null)
            {
                url = Mixer.Url ?? defaultUrl;
                CreateMixerControl();
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

        public List<CollectorScreen> Collectors
        {
            get { return collectors; }
            set { SetProperty(ref collectors, value); }
        }

        //public CollectorScreen Collector1
        //{
        //    get { return collector1; }
        //    set { SetProperty(ref collector1, value); }
        //}

        //public CollectorScreen Collector2
        //{
        //    get { return collector2; }
        //    set { SetProperty(ref collector2, value); }
        //}

        public List<SingleDosScreen> SingleDoses
        {
            get { return singleDoses; }
            set { SetProperty(ref singleDoses, value); }
        }

        public List<VolumeDosScreen> VolumeDoses
        {
            get { return volumeDoses; }
            set { SetProperty(ref volumeDoses, value); }
        }

        public List<PowderDosScreen> PowderDoses
        {
            get { return powderDoses; }
            set { SetProperty(ref powderDoses, value); }
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

        public void CreateMixerControl()
        {
            // todo: для других типов дозаторов сделать аналогично
            //Collectors = new List<CollectorScreen>((int)mixer.Collector);
            //for (int i = 0; i < (int)mixer.Collector; i++)
            //{
            //    Collectors.Add(new CollectorScreen(i+1));
            //}
            //Console.WriteLine(Collectors[0].Valves.Count);

            Common = new CommonScreen(Mixer.IsAirTemperatureSensor);
            Collectors = new List<CollectorScreen>();
            for (int i = 1; i <= (int)Mixer.MaxCollectors; i++)
            {
                Collectors.Add(new CollectorScreen(i));
            }
            VolumeDoses = new List<VolumeDosScreen>();
            for (int i = 1; i <= (int)Mixer.MaxVolumes; i++)
            {
                VolumeDoses.Add(new VolumeDosScreen(i));
            }
            PowderDoses = new List<PowderDosScreen>();
            for (int i = 1; i <= (int)Mixer.MaxPowders; i++)
            {
                PowderDoses.Add(new PowderDosScreen(i));
            }
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
            for (int i = 0; i < Mixer.Collector; i++)
            {
                Collectors[i].Update(ReadRegisters(Registers.Collectors[i], CollectorModbus.numberOfPoints));
            }
            for (int i = 0; i < Mixer.Volume; i++)
            {
                VolumeDoses[i].Update(ReadRegisters(Registers.VolumeDoses[i], VolumeDosModbus.numberOfPoints));
            }
            for (int i = 0; i < Mixer.Powder; i++)
            {
                PowderDoses[i].Update(ReadRegisters(Registers.PowderDoses[i], PowderDosModbus.numberOfPoints));
            }
        }

        private void UpdateClientState()
        {
            if (tcpClient != null)
            {
                IsConnected = (tcpClient.Client != null) && tcpClient.Connected;
            }
            else
            {
                IsConnected = false;
            }
        }

        public void WriteSingleRegister(RegisterValue registerValue)
        {
            if (!IsConnected) return;
            try
            {
                modbusMaster.WriteSingleRegister(slaveId, registerValue.Register, registerValue.Value);
                Console.WriteLine($"{DateTime.Now}\tWriteSingleRegister\tHR_{registerValue.Register} = {registerValue.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WriteSingleRegister32(RegisterValue32 registerValue)
        {
            if (!IsConnected) return;
            try
            {
                modbusMaster.WriteSingleRegister32(slaveId, registerValue.Register, registerValue.Value);
                Console.WriteLine($"{DateTime.Now}\tWriteSingleRegister32\tHR_{registerValue.Register} = {registerValue.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public ushort[] ReadRegisters(ushort startAddress, ushort numberOfPoints)
        {
            try
            {
                return modbusMaster.ReadHoldingRegisters(slaveId, startAddress, numberOfPoints);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"{DateTime.Now}\tError reading {numberOfPoints} registers with start address HR_{startAddress}");
                return new ushort[numberOfPoints];
            }
        }
        #endregion Methods
    }
}
