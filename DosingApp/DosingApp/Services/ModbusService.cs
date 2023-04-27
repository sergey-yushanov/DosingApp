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
        private const string url = "192.168.1.234";

        private TcpClient tcpClient;
        private IModbusMaster modbusMaster;
        private bool isConnected;

        private Mixer mixer;
        private CommonScreen common;
        private CollectorScreen collector1;
        private CollectorScreen collector2;
        //private SingleDosScreen singleDos;
        private VolumeDosScreen volumeDos;
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

        //public SingleDosScreen SingleDos
        //{
        //    get { return singleDos; }
        //    set { SetProperty(ref singleDos, value); }
        //}

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
            try
            {
                tcpClient.Connect(url, 502);
            }
            catch (Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Предупреждение", "Не удалось установить связь с ПЛК\nАдрес " + url + " не доступен", "Ok");
                //return;
            }
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
            //counter++;
            //fCounter++;

            //UInt32 www = 0x42c80083;
            //modbusMaster.WriteSingleRegister(slaveId, startAddressCommon, counter);

            //if (!BitConverter.IsLittleEndian)
            //{
            //    int bits = BitConverter.SingleToInt32Bits(fCounter);
            //    int revBits = BinaryPrimitives.ReverseEndianness(bits);
            //    fCounter = BitConverter.Int32BitsToSingle(revBits);
            //}

            //uint value32 = BitConverter.ToUInt32(BitConverter.GetBytes(fCounter), 0);
            //modbusMaster.WriteSingleRegister32(slaveId, (ushort)(startAddressCommon + 1), value32);

            //registersCommon = modbusMaster.ReadHoldingRegisters(slaveId, startAddressCommon, numRegistersCommon);
            

            //ushort tmpAddress = 16418;
            //ushort[] tmpRegisters = modbusMaster.ReadHoldingRegisters(slaveId, tmpAddress, 2);
            //float tmpValue = ModbusUtility.GetSingle(tmpRegisters[1], tmpRegisters[0]);

            //Console.WriteLine($"{DateTime.Now}\tHR_{(tmpAddress)},{tmpAddress + 1} = {tmpValue}");

            //;

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
        #endregion Methods
    }
}
