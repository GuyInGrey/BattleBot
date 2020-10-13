using System;
using WebSocketSharp.Server;

namespace BattleBot
{
    public class BattleBotServer : IDisposable
    {
        private WebSocketServer Server;

        public BattleBotServer(string url)
        {
            Server = new WebSocketServer(url);
            Server.AddWebSocketService<BattleBotServerBehaviour>("/");
            Server.Start();
        }

        public void Dispose()
        {
            if (!(Server is null) && Server.IsListening) { Server.Stop(); Server = null; }
            var a = new ServerArena();
        }
    }
}
