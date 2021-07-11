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

namespace DosingApp.ViewModels
{
    public class MixerControlViewModel : BaseViewModel
    {
        #region Attributes
        private readonly ClientWebSocket client;
        private readonly CancellationTokenSource cts;
        private IncomingMessage incomingMessage;
        private OutgoingMessage outgoingMessage;
        private String incomingMessageText;
        private String outgoingMessageText;
        private bool isConnected;

        public ICommand SendMessageCommand { get; protected set; }
        #endregion Attributes

        #region Constructor
        public MixerControlViewModel()
        {
            SendMessageCommand = new Command(SendMessageAsync);

            client = new ClientWebSocket();
            cts = new CancellationTokenSource();
            ConnectToServerAsync();
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

        //bool CanSendMessage(string message)
        //{
            //return IsConnected && !string.IsNullOrEmpty(message);
        //}
        #endregion Properties

        #region Commands
        //public Command SendMessage => sendMessageCommand ??
            //(sendMessageCommand = new Command<string>(SendMessageAsync, CanSendMessage));
        #endregion Commands

        #region Methods
        async void ConnectToServerAsync()
        {
            await client.ConnectAsync(new Uri("ws://192.168.11.1/ws"), cts.Token);

            UpdateClientState();

            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    UpdateClientState();

                    WebSocketReceiveResult result;
                    var message = new ArraySegment<byte>(new byte[4096]);
                    do
                    {
                        result = await client.ReceiveAsync(message, cts.Token);
                        var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                        IncomingMessageText = Encoding.UTF8.GetString(messageBytes);
                        
                        try
                        {
                            IncomingMessage = JsonConvert.DeserializeObject<IncomingMessage>(IncomingMessageText);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Invalide message format. {ex.Message}");
                        }

                    } while (!result.EndOfMessage);
                }
            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            void UpdateClientState()
            {
                IsConnected = client.State == WebSocketState.Open;
                //OnPropertyChanged(nameof(IsConnected));
                //sendMessageCommand.ChangeCanExecute();
                //Console.WriteLine($"Websocket state {client.State}");
            }
        }

        async void SendMessageAsync()
        {
            var msg = new OutgoingMessage
            {
                ShowSettings = false
            };

            OutgoingMessageText = JsonConvert.SerializeObject(msg, Formatting.Indented);

            var byteMessage = Encoding.UTF8.GetBytes(OutgoingMessageText);
            var segmnet = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);
            OutgoingMessageText = string.Empty;
        }
        #endregion Methods
    }
}
