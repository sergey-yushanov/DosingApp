using System;
using System.Linq;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Websocket.Client;
using DosingApp.Models.WebSocket;
using System.Collections.Generic;
using Newtonsoft.Json;
using DosingApp.Models.Screen;
using DosingApp.ViewModels;
using DosingApp.Models;
using DosingApp.DataContext;
using System.Collections.ObjectModel;

namespace DosingApp.Services
{
    public class WebSocketService : BaseViewModel
    {
        #region Attributes
        private IWebsocketClient client;
        private IncomingMessage incomingMessage;
        private OutgoingMessage outgoingMessage;
        private String incomingMessageText;
        private String outgoingMessageText;
        private bool isConnected;

        private Mixer mixer;
        private CommonScreen common;
        private CollectorScreen collector;
        private SingleDosScreen singleDos;
        //private ObservableCollection<JobComponentScreen> jobComponentScreens;
        #endregion Attributes

        #region Constructor
        public WebSocketService()
        {
            GetActiveMixer();
            if (Mixer != null)
            {
                CreateMixerControl(Mixer);
                ClientCreate();
                ConnectToServerAsync();
            }
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

        //public ObservableCollection<JobComponentScreen> JobComponentScreens
        //{
        //    get { return jobComponentScreens; }
        //    set { SetProperty(ref jobComponentScreens, value); }
        //}

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

        public void ClientCreate()
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

        public async void ConnectToServerAsync()
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
                        incomingMessageText = message.Text;
                        //Console.WriteLine(incomingMessageText);
                        incomingMessage = JsonConvert.DeserializeObject<IncomingMessage>(incomingMessageText);
                        UpdateIncomingData();
                    });

                Console.WriteLine("Websocket Starting...");
                await client.Start();
                Console.WriteLine("Websocket Started.");

            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void UpdateClientState()
        {
            IsConnected = client.IsRunning;
            //Console.WriteLine("Websocket Running: " + IsConnected);
        }

        public void UpdateIncomingData()
        {
            Collector.Update(incomingMessage.Collectors[0], incomingMessage.ShowSettings ?? false);
            SingleDos.Update(incomingMessage.Singles[0], incomingMessage.ShowSettings ?? false);
            Common.Update(incomingMessage.Common, incomingMessage.ShowSettings ?? false);
        }

        public async Task SendMessageAsync(OutgoingMessage outgoingMessage)
        {
            outgoingMessageText = JsonConvert.SerializeObject(outgoingMessage, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Console.WriteLine(outgoingMessageText);
            client.Send(outgoingMessageText);

            outgoingMessageText = string.Empty;
        }

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

        public void CommonLoopMessage(CommonLoop commonLoop)
        {
            var outgoingMessage = new OutgoingMessage() { Common = new Common { Loop = commonLoop } };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }

        public void CollectorLoopMessage(int collectorNumber, CollectorLoop collectorLoop)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Collectors = new List<Collector> {new Collector
                {
                    Number = collectorNumber,
                    Loop = collectorLoop
                }}
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }

        public void SingleLoopMessage(int singleDosNumber, SingleDosLoop singleDosLoop)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Singles = new List<SingleDos> {new SingleDos
                {
                    Number = singleDosNumber,
                    Loop = singleDosLoop
                }}
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
        }

        public void AllLoopMessage(CommonLoop commonLoop, CollectorLoop collectorLoop, SingleDosLoop singleDosLoop)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Common = new Common 
                { 
                    Loop = commonLoop 
                },
                Collectors = new List<Collector> {new Collector
                {
                    Number = 1,
                    Loop = collectorLoop
                }},
                Singles = new List<SingleDos> {new SingleDos
                {
                    Number = 1,
                    Loop = singleDosLoop
                }}
            };
            Task.Run(async () => await SendMessageAsync(outgoingMessage));
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

        public void SingleValveAdjustableMessage(int singleDosNumber, ValveAdjustable valveAdjustable)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Singles = new List<SingleDos> {new SingleDos
                {
                    Number = singleDosNumber,
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

        public void SingleFlowmeterMessage(int singleDosNumber, Flowmeter flowmeter)
        {
            var outgoingMessage = new OutgoingMessage()
            {
                Singles = new List<SingleDos> {new SingleDos
                {
                    Number = singleDosNumber,
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



        /*        public WebSocketService(RequestDelegate requestDelegate)
                {
                    this.requestDelegate = requestDelegate;
                }

                public async Task Invoke(HttpContext context)
                {
                    if (!context.WebSockets.IsWebSocketRequest)
                    {
                        await requestDelegate.Invoke(context);
                        return;
                    }

                    var token = context.RequestAborted;
                    var socket = await context.WebSockets.AcceptWebSocketAsync();

                    var guid = Guid.NewGuid().ToString();
                    sockets.TryAdd(guid, socket);

                    while (true)
                    {
                        if (token.IsCancellationRequested)
                            break;

                        var message = await GetMessageAsync(socket, token);
                        System.Console.WriteLine($"Received message - {message} at {DateTime.Now}");

                        if (string.IsNullOrEmpty(message))
                        {
                            if (socket.State != WebSocketState.Open)
                                break;

                            continue;
                        }

                        foreach (var s in sockets.Where(p => p.Value.State == WebSocketState.Open))
                            await SendMessageAsync(s.Value, message, token);
                    }

                    sockets.TryRemove(guid, out WebSocket redundantSocket);

                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Session ended", token);
                    socket.Dispose();
                }

                async Task<string> GetMessageAsync(WebSocket socket, CancellationToken token)
                {
                    WebSocketReceiveResult result;
                    var message = new ArraySegment<byte>(new byte[4096]);
                    string receivedMessage = string.Empty;

                    do
                    {
                        token.ThrowIfCancellationRequested();

                        result = await socket.ReceiveAsync(message, token);
                        var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                        receivedMessage = Encoding.UTF8.GetString(messageBytes);

                    } while (!result.EndOfMessage);

                    if (result.MessageType != WebSocketMessageType.Text)
                        return null;

                    return receivedMessage;
                }

                Task SendMessageAsync(WebSocket socket, string message, CancellationToken token)
                {
                    var byteMessage = Encoding.UTF8.GetBytes(message);
                    var segmnet = new ArraySegment<byte>(byteMessage);

                    return socket.SendAsync(segmnet, WebSocketMessageType.Text, true, token);
                }

                readonly RequestDelegate requestDelegate;
                ConcurrentDictionary<string, WebSocket> sockets = new ConcurrentDictionary<string, WebSocket>();
        */
    }
}
