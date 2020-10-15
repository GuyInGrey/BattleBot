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
            Client = new BattleBotClient
            {
                OnError = (error, details) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(error + "\n" + JsonConvert.SerializeObject(details));
                },
                OnReady = () =>
                {
                    Console.WriteLine("Ready! Token: " + Client.Token);
                    Client.JoinArena(new JoinArenaInfo()
                    {
                        Room = "GreyTestingRoom",
                        Password = "",
                        ClientName = "GuyInGrey's AI",
                        BattleCount = 1,
                        Team = 1,
                        RoomCapacity = 2,
                        StartTime = DateTime.Now.AddMinutes(5),
                    });
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
            Client.Start(@"ws://ldsgamers.com:3000");
            _ = RunPings();
            Art.TextFont(Art.CreateFont("Consolas", 0.2f));
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
            Art.Graphics.ScaleTransform(100, 100);
            Art.Background(PColor.CornflowerBlue);

            Art.Stroke(PColor.Black);
            Art.StrokeWeight(0.2f);
            Art.NoFill();

            var size = (float)Client.Arena.Size;
            Art.BeginShape();
            Art.Vertex(0, 0);
            Art.Vertex(size, 0);
            Art.Vertex(size, size);
            Art.Vertex(0, size);
            Art.EndShape(EndShapeType.Close);

            Art.NoStroke();
            Client.Arena.Obstacles.ForEach(o =>
            {
                Art.Fill(PColor.Grey);
                Art.Circle((float)o.X, (float)o.Y, 0.5f);
                Art.Fill(PColor.Black);
                Art.Text(o.ID, (float)o.X, (float)o.Y);
            });

            if (Client.Arena.ClientBot is object)
            {
                Art.Fill(PColor.White);
                Art.Circle((float)Client.Arena.ClientBot.X, (float)Client.Arena.ClientBot.Y, 0.5f);
                Art.Fill(PColor.Black);
                Art.Text("BOT\n" + Client.Arena.ClientBot.ID, (float)Client.Arena.ClientBot.X, (float)Client.Arena.ClientBot.Y);
            }
        }

        private async Task RunPings()
        {
            while (Client?.Socket is object)
            {
                Title($"Latency: {await Client.Socket.GetLatency()} ms; FPS: " + FrameRateCurrent);
                await Task.Delay(500);
            }
        }
    }
}
