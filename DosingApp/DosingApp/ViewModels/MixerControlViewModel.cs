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

namespace DosingApp.ViewModels
{
    public class MixerControlViewModel : BaseViewModel
    {
        #region Attributes
        private IWebsocketClient client;
        private IncomingMessage incomingMessage;
        private OutgoingMessage outgoingMessage;
        private String incomingMessageText;
        private String outgoingMessageText;
        private bool isConnected;

        //private bool isFirstMessage;

        private Mixer mixer;

        private string title;

        // mixer parts
        //private MixerControl mixerControl;

        //private List<CollectorScreen> collectors;
        //private CollectorScreen collector;

        private CommonScreen common;
        private CollectorScreen collector;

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

        public ICommand CollectorValveOpenCommand { get; protected set; }
        public ICommand CollectorValveCloseCommand { get; protected set; }

        public ICommand CollectorValveAdjustableOpenCommand { get; protected set; }
        public ICommand CollectorValveAdjustableCloseCommand { get; protected set; }
        public ICommand CollectorValveAdjustableSetpointCommand { get; protected set; }
        public ICommand CollectorFlowmeterNullifyCommand { get; protected set; }

        public ICommand ValveAdjustableOpenCommand { get; protected set; }
        public ICommand ValveAdjustableCloseCommand { get; protected set; }
        public ICommand ValveAdjustableSetpointCommand { get; protected set; }
        public ICommand FlowmeterNullifyCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public MixerControlViewModel()
        {
            //CollectorNumber = 1;

            // screen
            GetActiveMixer();
            if (Mixer != null)
            {
                Title = (Mixer != null) ? Mixer.Name + " (Ручной режим)" : "Не выбрана активная установка";

                // create template for mixer control
                CreateMixerControl(Mixer);

                // websocket
                //isFirstMessage = true;
                //showSettings = true;
                //SendMessageCommand = new Command(SendSettingsMessage);
                ClientCreate();
                ConnectToServerAsync();

                CollectorValveOpenCommand = new Command(CollectorValveOpen);
                CollectorValveCloseCommand = new Command(CollectorValveClose);

                CollectorValveAdjustableOpenCommand = new Command(CollectorValveAdjustableOpen);
                CollectorValveAdjustableCloseCommand = new Command(CollectorValveAdjustableClose);
                CollectorValveAdjustableSetpointCommand = new Command(CollectorValveAdjustableSetpoint);
                CollectorFlowmeterNullifyCommand = new Command(CollectorFlowmeterNullify);

                ValveAdjustableOpenCommand = new Command(ValveAdjustableOpen);
                ValveAdjustableCloseCommand = new Command(ValveAdjustableClose);
                ValveAdjustableSetpointCommand = new Command(ValveAdjustableSetpoint);
                FlowmeterNullifyCommand = new Command(FlowmeterNullify);
            }
        }
        #endregion Constructor

        #region Properties
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public CollectorScreen Collector
        {
            get { return collector; }
            set { SetProperty(ref collector, value); }
        }

        public CommonScreen Common
        {
            get { return common; }
            set { SetProperty(ref common, value); }
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

        public Mixer Mixer
        {
            get
            {
                //MixerName = (mixer != null) ? mixer.Name : "Не выбрана активная установка";
                return mixer;
            }
            set { SetProperty(ref mixer, value); }
        }

        public IncomingMessage IncomingMessage
        {
            get { return incomingMessage; }
            set { SetProperty(ref incomingMessage, value); }
        }

        public OutgoingMessage OutgoingMessage
        {
            get { return outgoingMessage; }
            set { SetProperty(ref outgoingMessage, value); }
        }

        public String IncomingMessageText
        {
            get { return incomingMessageText; }
            set { SetProperty(ref incomingMessageText, value); }
        }

        public String OutgoingMessageText
        {
            get { return outgoingMessageText; }
            set { SetProperty(ref outgoingMessageText, value); }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { SetProperty(ref isConnected, value); }
        }
        #endregion Properties

        #region Commands
        private void CollectorValveOpen(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            CollectorValveMessage(Collector.Number, new Valve { Number = valveScreen.Number, CommandOpen = true });
        }

        private void CollectorValveClose(object valveInstance)
        {
            ValveScreen valveScreen = valveInstance as ValveScreen;
            CollectorValveMessage(Collector.Number, new Valve { Number = valveScreen.Number, CommandClose = true });
        }

        private void CollectorValveAdjustableOpen(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            CollectorValveAdjustableMessage(Collector.Number, new ValveAdjustable { CommandOpen = true });
        }

        private void CollectorValveAdjustableClose(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            CollectorValveAdjustableMessage(Collector.Number, new ValveAdjustable { CommandClose = true });
        }

        private void CollectorValveAdjustableSetpoint(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            CollectorValveAdjustableMessage(Collector.Number, new ValveAdjustable { Setpoint = (float)valveAdjustableScreen.Setpoint });
        }

        private void CollectorFlowmeterNullify(object flowmeterInstance)
        {
            //FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            CollectorFlowmeterMessage(Collector.Number, new Flowmeter { NullifyVolume = true });
        }

        private void ValveAdjustableOpen(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ValveAdjustableMessage(new ValveAdjustable { CommandOpen = true });
        }

        private void ValveAdjustableClose(object valveAdjustableInstance)
        {
            //ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ValveAdjustableMessage(new ValveAdjustable { CommandClose = true });
        }

        private void ValveAdjustableSetpoint(object valveAdjustableInstance)
        {
            ValveAdjustableScreen valveAdjustableScreen = valveAdjustableInstance as ValveAdjustableScreen;
            ValveAdjustableMessage(new ValveAdjustable { Setpoint = valveAdjustableScreen.Setpoint });
        }

        private void FlowmeterNullify(object flowmeterInstance)
        {
            //FlowmeterScreen flowmeterScreen = flowmeterInstance as FlowmeterScreen;
            FlowmeterMessage(new Flowmeter { NullifyVolume = true });
        }
        #endregion Commands

        #region Methods
        void ClientCreate()
        {
            var factory = new Func<ClientWebSocket>(() => new ClientWebSocket
            {
                Options =
                {
                    KeepAliveInterval = TimeSpan.FromSeconds(5)
                }
            });
            var url = new Uri("ws://192.168.11.1/ws");
            client = new WebsocketClient(url, factory);
        }
        
        async void ConnectToServerAsync()
        {
            await Task.Factory.StartNew(async () =>
            {
                client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                client
                    .ReconnectionHappened
                    .Subscribe(info =>
                    {
                        Console.WriteLine($"Websocket Reconnection happened, type: {info.Type}");
                        UpdateClientState();
                    });

                client
                    .DisconnectionHappened
                    .Subscribe(x =>
                    {
                        UpdateClientState();
                    });

                client
                    .MessageReceived
                    .Subscribe(message =>
                    {
                        UpdateClientState();
                        IncomingMessageText = message.Text;
                        IncomingMessage = JsonConvert.DeserializeObject<IncomingMessage>(IncomingMessageText);
                        UpdateIncomingData();
                    });

                Console.WriteLine("Websocket Starting...");
                await client.Start();
                Console.WriteLine("Websocket Started.");

            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        void UpdateClientState()
        {
            IsConnected = client.IsRunning;
        }

        void UpdateIncomingData()
        {
            Collector.Update(IncomingMessage.Collectors[0], IncomingMessage.ShowSettings ?? false);
            Common.Update(IncomingMessage.Common, IncomingMessage.ShowSettings ?? false);

            
            //for (int i = 0; i < 4; i++)
            //{
            //    var valve = Collector.Valves.FirstOrDefault(v => v.Number == IncomingMessage.Collectors[0].Valves[i].Number);
            //    valve.Command = (bool)IncomingMessage.Collectors[0].Valves[i].Command;
            //}

            //Collector.ValveAdjustable.Position = (float)IncomingMessage.Collectors[0].ValveAdjustable.Position;
            //Collector.ValveAdjustable.Setpoint = (float)IncomingMessage.Collectors[0].ValveAdjustable.Setpoint;

            //if (isFirstMessage)
            //{
            //    Collector.InitNew(IncomingMessage.Collectors[0], IncomingMessage.ShowSettings ?? false);
            //    Common.InitNew(IncomingMessage.Common, IncomingMessage.ShowSettings ?? false);
            //    isFirstMessage = false;
            //}
        }

        private async Task SendMessageAsync(OutgoingMessage outgoingMessage)
        {
            OutgoingMessageText = JsonConvert.SerializeObject(outgoingMessage, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine(OutgoingMessageText);
            client.Send(OutgoingMessageText);

            OutgoingMessageText = string.Empty;
        }

        //void SendSettingsMessage()
        //{
        //    showSettings = !showSettings;
        //    var outgoingMessage = new OutgoingMessage
        //    {
        //        ShowSettings = showSettings
        //    };

        //    Task.Run(async () => await SendMessageAsync(outgoingMessage));
        //}

        public void WebsocketClientExit()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
            //client.Dispose();
            Console.WriteLine("Websocket Stoped.");
        }

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
            Common = new CommonScreen();
        }

        public void CollectorValveMessage(int collectorNumber, Valve valve)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Collectors = new List<Collector> {new Collector
                {
                    Number = collectorNumber,
                    Valves = new List<Valve> { valve }
                }}
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }

        public void CollectorValveAdjustableMessage(int collectorNumber, ValveAdjustable valveAdjustable)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Collectors = new List<Collector> {new Collector
                {
                    Number = collectorNumber,
                    ValveAdjustable = valveAdjustable
                }}
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }

        public void CollectorFlowmeterMessage(int collectorNumber, Flowmeter flowmeter)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Collectors = new List<Collector> {new Collector
                {
                    Number = collectorNumber,
                    Flowmeter = flowmeter
                }}
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }

        public void ValveAdjustableMessage(ValveAdjustable valveAdjustable)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Common = new Common
                {
                    ValveAdjustable = valveAdjustable
                }
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }

        public void FlowmeterMessage(Flowmeter flowmeter)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Common = new Common
                {
                    Flowmeter = flowmeter
                }
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }
        #endregion Methods
    }
}
