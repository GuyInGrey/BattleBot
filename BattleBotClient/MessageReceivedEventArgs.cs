using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBotClient
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string Content;
        public NetworkClient Client;

        public MessageReceivedEventArgs(string content, NetworkClient client)
        {
            Content = content;
            Client = client;
        }
    }
}
