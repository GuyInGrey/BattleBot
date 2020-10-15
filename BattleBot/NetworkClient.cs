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
        private ClientWebSocket ClientSocket;

        public event EventHandler<SocketMessageEventArgs> OnMessageReceived;
        public event EventHandler<SocketMessageEventArgs> OnMessageSent;
        public event EventHandler OnClientConnected;

        public string Address { get; private set; }

        public NetworkClient()
        {
            ClientSocket = new ClientWebSocket();
        }

        /// <summary>
        /// The NetworkClient connects to the server and begins listening for incoming messages.
        /// </summary>
        /// <param name="url">The URL of the server to connect to, WebSocket complient.</param>
        /// <returns>Returns whether the connection was successful or not.</returns>
        public async Task<bool> ConnectAndListen(string url)
        {
            try
            {
                await ClientSocket.ConnectAsync(new Uri(url), CancellationToken.None);
                OnClientConnected?.Invoke(this, null);
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
            while (ClientSocket.State == WebSocketState.Open)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        WebSocketReceiveResult result = null;
                        do
                        {
                            var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                            result = await ClientSocket.ReceiveAsync(messageBuffer, CancellationToken.None);
                            ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);

                        } while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var data = Encoding.UTF8.GetString(ms.ToArray());
                            ms.Seek(0, SeekOrigin.Begin);
                            ms.Position = 0;

                            OnMessageReceived?.Invoke(this, new SocketMessageEventArgs(data));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}\n\n{e.StackTrace}");
                }
            }
        }

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="content">The message to send.</param>
        /// <returns>Returns whether the message was successfuly sent or not.</returns>
        public bool SendMessage(string content)
        {
            if (ClientSocket.State != WebSocketState.Open) { return false; }
            try
            {
                ClientSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(content)), 
                    WebSocketMessageType.Text, true, CancellationToken.None);
                OnMessageSent?.Invoke(this, new SocketMessageEventArgs(content));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n\n{e.StackTrace}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Disconnects the client from the server and safely disposes of the NetworkClient.
        /// </summary>
        public void Dispose()
        {
            ClientSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Client Disposed", CancellationToken.None).GetAwaiter().GetResult();
            ClientSocket.Dispose();
        }

        /// <summary>
        /// Gets the latency between the client and the server.
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetLatency()
        {
            if (Address is null) { return -1; }

            var modified = Address
                .Replace("ws://", "")
                .Replace("wss://", "")
                .Split(':')[0];

            var ping = new Ping();
            var response = ping.Send(modified, 15000);
            return response.RoundtripTime;
        }
    }
}
