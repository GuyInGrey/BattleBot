using System;
using System.Dynamic;
using Newtonsoft.Json;

namespace BattleBot
{
    public class BattleBotClient
    {
        private NetworkClient Client;
        private string Token;

        public Action<string, dynamic> OnError;
        public Action<string, int> OnGameEnd;
        public Action OnReady;

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

                    break;
                case "ready":
                    Token = payload;
                    OnReady?.Invoke();
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
