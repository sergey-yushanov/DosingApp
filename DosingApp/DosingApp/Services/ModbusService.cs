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

namespace DosingApp.Services
{
    public class ModbusService : BaseViewModel
    {
        #region Attributes
        private const byte slaveId = 1;

        private TcpClient tcpClient;
        private IModbusMaster modbusMaster;
        private bool isConnected;

        //private ushort[] registersCommon;
        //private ushort[,] registersCollector;
        //private ushort[] registersSingleDos;

        
        //private ushort numRegistersCommon;
        //private ushort numRegistersCollector;
        //private ushort numRegistersSingleDos;

        //private ushort startAddressCommon;
        //private ushort startAddressCollector;
        //private ushort startAddressSingleDos;

        //private bool isConnected;

        private Mixer mixer;
        private CommonScreen common;
        private CollectorScreen collector;
        private SingleDosScreen singleDos;


        private CollectorModbus collectorModbus;

        //private ushort counter;
        //private float fCounter;
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

            //collectorModbus
            
            //numRegistersCommon = 24;
            //numRegistersCollector = 20;
            //numRegistersSingleDos = 10;

            //startAddressCommon = 16384;
            //startAddressCollector = 16384;
            //startAddressSingleDos = 16384;

            //registersCommon = new ushort[numRegistersCommon];
            //counter = 0;
            //fCounter = 0;
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

        public CollectorModbus CollectorModbus
        {
            get { return collectorModbus; }
            set { SetProperty(ref collectorModbus, value); }
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

        //public ushort TestRegister
        //{
        //    get 
        //    {
        //        return registersCommon[numRegistersCommon-1];
        //    }
        //}
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

            Collector = new CollectorScreen(1);
            SingleDos = new SingleDosScreen(1);
            Common = new CommonScreen();


        }

        public void MasterCreate()
        {
            tcpClient = new TcpClient();
            tcpClient.Connect("192.168.3.5", 502);
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


        public void SendMessage(OutgoingMessage outgoingMessage)
        {
            String outgoingMessageText = JsonConvert.SerializeObject(outgoingMessage, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine(outgoingMessageText);

            Console.WriteLine(outgoingMessage.Collectors[0].ValveAdjustable.CommandClose);

            ushort value = Convert.ToUInt16(outgoingMessage.Collectors[0].ValveAdjustable.CommandOpen);
            //WriteSingleRegister(16600, value);

            //outgoingMessageText = JsonConvert.SerializeObject(outgoingMessage, Formatting.Indented, new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //});
            //Console.WriteLine(outgoingMessageText);
            //client.Send(outgoingMessageText);

            //outgoingMessageText = string.Empty;
        }


        public IModbusMaster GetModbusMaster()
        {
            return modbusMaster;
        }

        public void WriteSingleRegister(RegisterValue registerValue)
        {
            modbusMaster.WriteSingleRegister(slaveId, registerValue.Register, registerValue.Value);
            Console.WriteLine($"{DateTime.Now}\tWriteSingleRegister\tHR_{registerValue.Register} = {registerValue.Value}");
        }

        public void WriteSingleRegister32(RegisterValue32 registerValue)
        {
            modbusMaster.WriteSingleRegister32(slaveId, registerValue.Register, registerValue.Value);
            Console.WriteLine($"{DateTime.Now}\tWriteSingleRegister32\tHR_{registerValue.Register} = {registerValue.Value}");
        }

        public void CollectorValveAdjustableMessage(int collectorNumber, ValveAdjustable valveAdjustable)
        {
            var collectorModbus = new CollectorModbus();
            collectorModbus.ValveAdjustableOpen((ushort)collectorNumber);


            //var outgoingMessage = new OutgoingMessage()
            //{
            //    Collectors = new List<Collector> {new Collector
            //    {
            //        Number = collectorNumber,
            //        ValveAdjustable = valveAdjustable
            //    }}
            //};
            //SendMessage(outgoingMessage);

            WriteSingleRegister(new CollectorModbus().ValveAdjustableOpen((ushort)collectorNumber));
        }
        #endregion Methods
    }
}
