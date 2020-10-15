using System;
using System.Threading;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BattleBot
{
    public class BattleBotServerBehaviour : WebSocketBehavior
    {
        public string Token;

        protected override void OnMessage(MessageEventArgs e)
        {
            //Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine(e.Data);
            Thread.Sleep(1000);
            SendState();
        }

        protected override void OnOpen()
        {
            Token = ID + "-" + DateTime.Now.Ticks;

            var template =
                "{ \"type\": \"ready\", \"payload\": \"" + Token  + "\" }";

            Send(template);
            SendState();
        }

        public void SendState()
        {
            dynamic state = new
            {
                type = "state",
                payload = (dynamic)new
                {
                    turn = 0,
                    nextTurn = 1,
                    playersRemaining = 5,
                    clientBot = (dynamic)new
                    {
                        id = "Grey's Bot (ID)",
                        hp = 2.5m,
                        x = 0,
                        y = 1,
                        r = 2,
                        name = "Grey's Bot",
                        heading = 5m,
                        scanner = 1m,
                    },
                    damage = (dynamic)new dynamic[0],
                    shots = (dynamic)new dynamic[0],
                    botsDestroyed = (dynamic)new dynamic[0],
                    objectsDestroyed = (dynamic)new dynamic[0],
                    scanResults = (dynamic)new dynamic[0],
                    scannedBy = (dynamic)new dynamic[0],
                },
            };

            Send(JsonConvert.SerializeObject(state));
        }
    }
}
