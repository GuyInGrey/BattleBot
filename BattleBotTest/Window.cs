using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Processing;

namespace BattleBot.Tests
{
    public class Window : ProcessingCanvas
    {
        BattleBotClient Client;
        BattleBotServer Server;

        public Window()
        {
            CreateCanvas(1000, 1000, 30);
        }

        public void Setup()
        {
            Server = new BattleBotServer("ws://localhost:3000");
            Client = new BattleBotClient
            {
                OnError = (error, details) =>
                {
                    Console.WriteLine(error + "\n" + JsonConvert.DeserializeObject(details));
                },
                OnReady = () =>
                {
                    Console.WriteLine("Ready! Token: " + Client.Token);
                },
                OnGameEnd = (winner, rounds) =>
                {
                    Console.WriteLine($"{winner} has won the game! ({rounds} rounds)");
                },
                OnTurn = (turnInfo, response) =>
                {
                    response.SetMovement(MovementType.MoveForward, 3.5m);
                    response.SetScanner(10m);
                    response.SetWeapon(WeaponType.Mortar, 90m, 2m);
                },
            };
            Client.Socket.OnMessageSent += Socket_OnMessageSend;
            Client.Socket.OnMessageReceived += Socket_OnMessageReceived;
            Client.Start(@"ws://localhost:3000");
            _ = RunPings();
        }

        private void Socket_OnMessageReceived(object sender, SocketMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"[{DateTime.Now}]");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(e.Content);
        }

        private void Socket_OnMessageSend(object sender, SocketMessageEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"[{DateTime.Now}]");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(e.Content);
        }

        private void Draw(float delta)
        {
            Art.Background(PColor.CornflowerBlue);

            Art.Circle(1, 1, 0.5f);
            Art.Graphics.ScaleTransform(10, 10);
            Client.Arena.Obstacles.ForEach(o =>
            {
                Art.Circle((float)o.X, (float)o.Y, 0.5f);
            });
        }

        private async Task RunPings()
        {
            while (Client?.Socket is object)
            {
                Title($"Latency: {await Client.Socket.GetLatency()} ms");
                await Task.Delay(500);
            }
        }
    }
}
