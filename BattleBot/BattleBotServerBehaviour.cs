using System;
using System.Threading;
using WebSocketSharp.Server;

namespace BattleBot
{
    public class BattleBotServerBehaviour : WebSocketBehavior
    {
        public string Token;

        protected override void OnOpen()
        {
            Token = ID + "-" + DateTime.Now.Ticks;

            var template =
                "{ \"type\": \"ready\", \"payload\": \"" + Token  + "\" }";

            Send(template);
        }
    }
}
