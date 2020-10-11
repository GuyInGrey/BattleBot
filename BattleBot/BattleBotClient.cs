using System;

namespace BattleBot
{
    public class BattleBotClient
    {
        private NetworkClient Client;
        private string Token;

        public Action<string, dynamic> OnError;
        public Action<string, int> OnGameEnd;
        public Action OnReady;
        public Action<TurnInfo> Turn;
        public Arena Arena;

        public BattleBotClient(string url)
        {
            Client = new NetworkClient();
            Client.OnMessageReceived += Client_OnMessageReceived;
            _ = Client.BeginListening(url); // TO CHANGE: CORRECT PORT
        }

        private void Client_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var data = e.Content.FromJson();
            HandleMessage(data.type, data.payload);
        }

        public void HandleMessage(string type, dynamic payload)
        {
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
                    Turn?.Invoke(turn);
                    break;

            }
        }

        public void SendMessage(string type, string payload)
        {
            dynamic obj = new { token = Token, type, payload, };
            Client.SendMessage(Extensions.ToJson(obj));
        }
    }
}
