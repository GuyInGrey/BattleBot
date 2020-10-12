using System;
using System.Dynamic;
using System.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BattleBot
{
    public class BattleBotClient
    {
        private NetworkClient Client;
        public string Token { get; private set; }

        public Action<string, dynamic> OnError;
        public Action<string, int> OnGameEnd;
        public Action OnReady;
        public Action<TurnInfo> OnTurn;

        public Arena Arena;

        public BattleBotClient()
        {
            Client = new NetworkClient();
            Client.OnMessageReceived += Client_OnMessageReceived;
        }

        public void Start(string url)
        {
            _ = Client.BeginListening(url);
        }

        private void Client_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            dynamic data = Json.Decode(e.Content);
            var type = data.Type;
            var payload = data.Payload;

            switch (type)
            {
                case "end":
                    OnGameEnd?.Invoke(payload.winner, payload.rounds);
                    break;
                case "error":
                    OnError?.Invoke(payload.error, payload.data);
                    break;
                case "joined":
                    Arena = new Arena
                    {
                        Size = payload.size,
                        Capacity = payload.capacity
                    };
                    foreach (var obstacle in payload.obstacles)
                    {
                        Arena.Obstacles.Add(WorldObject.FromDynamic(obstacle));
                    }
                    Arena.ClientBot = Bot.FromDynamic(payload.clientBot);
                    break;
                case "ready":
                    Token = payload;
                    OnReady?.Invoke();
                    break;
                case "state":
                    Arena.Turn = payload.turn;
                    Arena.NextTurn = payload.nextTurn;
                    Arena.PlayersRemaining = payload.playersRemaining;
                    Arena.ClientBot = Bot.FromDynamic(payload.clientBot);
                    var turn = TurnInfo.FromDynamic(payload);
                    OnTurn?.Invoke(turn);
                    break;

            }
        }

        public void SendMessage(string type, string payload)
        {
            dynamic obj = new { token = Token, type, payload, };
            Client.SendMessage(JsonConvert.SerializeObject(obj));
        }
    }
}
