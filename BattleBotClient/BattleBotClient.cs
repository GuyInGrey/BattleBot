using System;
using System.Dynamic;
using Newtonsoft.Json;

namespace BattleBotClient
{
    public class BattleBotClient
    {
        private NetworkClient Client;
        private string Token;

        public Action<string, string> OnError;

        public BattleBotClient(string ip)
        {
            Client = new NetworkClient();
            Client.OnMessageReceived += Client_OnMessageReceived;
            Client.BeginListening(""); // TO CHANGE: CORRECT PORT
        }

        private void Client_OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(e.Content);
            HandleMessage(data.type, data.payload);
        }

        public void HandleMessage(string type, dynamic payload)
        {
            switch (type)
            {
                case "error":
                    string error = payload.error;
                    string errorContent = JsonConvert.SerializeObject(payload.data);
                    OnError?.Invoke(error, errorContent);
                    break;
                case "ready":
                    Token = payload;
                    break;
            }
        }

        public void SendMessage(string type, string payload)
        {
            dynamic obj = new ExpandoObject();
            obj.token = Token;
            obj.type = type;
            obj.payload = payload;
            Client.SendMessage(JsonConvert.SerializeObject(obj));
        }
    }
}
