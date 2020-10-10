using System;

namespace BattleBot
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string Content;

        public MessageReceivedEventArgs(string content)
        {
            Content = content;
        }
    }
}
