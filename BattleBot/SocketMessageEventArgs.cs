using System;

namespace BattleBot
{
    public class SocketMessageEventArgs : EventArgs
    {
        public string Content;

        public SocketMessageEventArgs(string content)
        {
            Content = content;
        }
    }
}
