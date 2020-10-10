using System;
using System.IO;
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

                        var data = Encoding.UTF8.GetString(ms.ToArray());
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Position = 0;

                        OnMessageReceived?.Invoke(this, new MessageReceivedEventArgs(data));
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
    }
}
