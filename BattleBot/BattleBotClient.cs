using System;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace BattleBot
{
    public class BattleBotClient
    {
        public NetworkClient Socket;
        public string Token { get; private set; }

        public Action<string, dynamic> OnError;
        public Action<string, int> OnGameEnd;
        public Action OnReady;
        public Action<TurnInfo, TurnResponse, Bot> OnTurn;

        public ClientArena Arena = new ClientArena();

        public BattleBotClient()
        {
            Socket = new NetworkClient();
        }

        public void Start(string url)
        {
            Socket.OnMessageReceived += Client_OnMessageReceived;
            _ = Socket.ConnectAndListen(url);
        }

        private void Client_OnMessageReceived(object sender, SocketMessageEventArgs e)
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
                    Arena = new ClientArena()
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
                    var turn = (TurnInfo)TurnInfo.FromDynamic(payload);
                    foreach (var obj in turn.DestroyedObjects)
                    {
                        Arena.Obstacles.RemoveAll(o => o.ID == obj);
                    }
                    var response = new TurnResponse();
                    OnTurn?.Invoke(turn, response, Arena.ClientBot);
                    SendMessage("turn", response.GetObject(Arena.NextTurn));
                    break;
            }
        }

        public void SendMessage(string type, dynamic payload)
        {
            dynamic obj = new { token = Token, type, payload, };
            Socket.SendMessage(JsonConvert.SerializeObject(obj));
        }

        public void JoinArena(JoinArenaInfo info)
        {
            SendMessage("join", info.ToDynamic());
        }
    }
}