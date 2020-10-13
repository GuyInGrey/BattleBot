using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleBot
{
    public class NetworkClient : IDisposable
    {
        private ClientWebSocket Client;
        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;
        public string Address { get; private set; }

        public NetworkClient()
        {
            Client = new ClientWebSocket();
        }

        public NetworkClient(ClientWebSocket client)
        {
            Client = client;
        }

        public async Task<bool> BeginListening(string url)
        {
            try
            {
                await Client.ConnectAsync(new Uri(url), CancellationToken.None);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Client connected.");
                Address = url;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n\n{e.StackTrace}");
                return false;
            }
            _ = Listen();
            return true;
        }

        private async Task Listen()
        {
            while (Client.State == WebSocketState.Open)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        WebSocketReceiveResult result = null;
                        do
                        {
                            var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                            result = await Client.ReceiveAsync(messageBuffer, CancellationToken.None);
                            ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);

                        } while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var data = Encoding.UTF8.GetString(ms.ToArray());
                            ms.Seek(0, SeekOrigin.Begin);
                            ms.Position = 0;

                            OnMessageReceived?.Invoke(this, new MessageReceivedEventArgs(data));
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine(data);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}\n\n{e.StackTrace}");
                }
            }
        }

        public bool SendMessage(string content)
        {
            if (Client.State != WebSocketState.Open) { return false; }
            try
            {
                Client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(content)), 
                    WebSocketMessageType.Text, true, CancellationToken.None);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(content);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n\n{e.StackTrace}");
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            Client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Client Disposed", CancellationToken.None).GetAwaiter().GetResult();
            Client.Dispose();
        }

        public async Task<long> GetLatency()
        {
            if (Address is null) { return -1; }

            var modified = Address
                .Replace("ws://", "")
                .Replace("wss://", "")
                .Split(':')[0];

            var ping = new Ping();
            var response = ping.Send(modified.Replace("ws://", ""), 15000);
            return response.RoundtripTime;
        }
    }
}
