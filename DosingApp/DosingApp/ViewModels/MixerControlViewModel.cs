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
//using Plugin.DeviceInfo;
using Newtonsoft.Json;
using DosingApp.Models;
using System.Windows.Input;
using Websocket.Client;

namespace DosingApp.ViewModels
{
    public class MixerControlViewModel : BaseViewModel
    {
        #region Attributes
        //private readonly ClientWebSocket client;
        //private readonly CancellationTokenSource cts;
        private IWebsocketClient client;
        private IncomingMessage incomingMessage;
        private OutgoingMessage outgoingMessage;
        private String incomingMessageText;
        private String outgoingMessageText;
        private bool isConnected;

        public ICommand SendMessageCommand { get; protected set; }

        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        #endregion Attributes

        #region Constructor
        public MixerControlViewModel()
        {
            SendMessageCommand = new Command(SendSettingsMessage);

            //client = new ClientWebSocket();
            //cts = new CancellationTokenSource();
            //ConnectToServerAsync();

            ClientCreate();
            ConnectToServerAsync();
            //ConnectToServer();
        }
        #endregion Constructor

        #region Properties
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
        //public bool IsConnected => client.State == WebSocketState.Open;
        #endregion Properties

        #region Commands
        //public Command SendMessage => sendMessageCommand ??
        //(sendMessageCommand = new Command<string>(SendMessageAsync, CanSendMessage));
        #endregion Commands

        #region Methods
        void ClientCreate()
        {
            var factory = new Func<ClientWebSocket>(() => new ClientWebSocket
            {
                Options =
                {
                    KeepAliveInterval = TimeSpan.FromSeconds(5)
                    //Proxy = ...
                    //ClientCertificates = ...
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
                client.ReconnectionHappened.Subscribe(info =>
                    Console.WriteLine($"Websocket Reconnection happened, type: {info.Type}"));

                client
                    .MessageReceived
                    .Subscribe(message =>
                    {
                        IncomingMessageText = message.Text;                        
                        IncomingMessage = JsonConvert.DeserializeObject<IncomingMessage>(IncomingMessageText);
                        //Console.WriteLine(IncomingMessageText);
                    });

                Console.WriteLine("Websocket Starting...");
                await client.Start();
                Console.WriteLine("Websocket Started.");

                //ExitEvent.WaitOne();
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        //void UpdateClientState()
        //{
        //IsConnected = client.State == WebSocketState.Open;

        //OnPropertyChanged(nameof(IsConnected));
        //Console.WriteLine($"Websocket state {client.State}");
        //}

        //async void ConnectToServerAsync()
        //{
        //    await client.ConnectAsync(new Uri("ws://192.168.11.1/ws"), cts.Token);
        //    UpdateClientState();

        //    await Task.Factory.StartNew(async () =>
        //    {
        //        while (IsConnected)
        //        {
        //            await ReadMessageAsync();
        //            UpdateClientState();
        //        }
        //    }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        //}

        /*async Task ReadMessageAsync()
        {
            WebSocketReceiveResult result;
            var message = new ArraySegment<byte>(new byte[4096]);
            do
            {
                result = await client.ReceiveAsync(message, cts.Token);
                if (result.MessageType != WebSocketMessageType.Text)
                    return;
                
                var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                IncomingMessageText = Encoding.UTF8.GetString(messageBytes);
                IncomingMessage = JsonConvert.DeserializeObject<IncomingMessage>(IncomingMessageText);

                //try
                //{
                //    IncomingMessage = JsonConvert.DeserializeObject<IncomingMessage>(IncomingMessageText);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"Invalide message format. {ex.Message}");
                //}
            }
            while (!result.EndOfMessage);
        }*/

        /*async void SendMessageAsync(OutgoingMessage message)
        {
            //var msg = new OutgoingMessage
            //{
            //ShowSettings = false
            //};

            UpdateClientState();
            if (!IsConnected)
                return;

            OutgoingMessageText = JsonConvert.SerializeObject(message, Formatting.Indented);

            var byteMessage = Encoding.UTF8.GetBytes(OutgoingMessageText);
            var segmnet = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);
            OutgoingMessageText = string.Empty;
        }*/

        private async Task SendMessageAsync(OutgoingMessage outgoingMessage)
        {
            OutgoingMessageText = JsonConvert.SerializeObject(outgoingMessage, Formatting.Indented);
            client.Send(OutgoingMessageText);

            //var byteMessage = Encoding.UTF8.GetBytes(OutgoingMessageText);
            //var segmnet = new ArraySegment<byte>(byteMessage);
            //await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);
            
            OutgoingMessageText = string.Empty;
        }

        void SendSettingsMessage()
        {
            var outgoingMessage = new OutgoingMessage
            {
                ShowSettings = false
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }

        public void WebsocketClientExit()
        {
            ExitEvent.Set();
            client.Dispose();
            Console.WriteLine("Websocket Stoped.");
        }
        #endregion Methods
    }
}
