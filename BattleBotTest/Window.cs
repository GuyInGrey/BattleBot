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
        bool Debug;
        Vector2 LastFirePosition;

        public Window(bool debug)
        {
            Debug = debug;
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
                OnTurn = (turnInfo, response, clientBot) =>
                {
                    response.SetScanner(60m);
                    var nearestObstacle = Client.Arena.Obstacles.GetNearest(Client.Arena.ClientBot);
                    if (Debug) { Console.WriteLine($"BotID: {clientBot.ID}; Nearest obstacle: {nearestObstacle.ID}"); }

                    var angle = clientBot.Position.AngleTo(nearestObstacle.Position);
                    angle = (angle + 180) % 360;

                    if (clientBot.Heading != angle)
                    {
                        response.SetMovement(MovementType.Turn, angle);
                        if (Debug) { Console.WriteLine($"Turning to {nearestObstacle.ID}; Angle {angle}"); }
                    }
                    else
                    {
                        response.SetMovement(MovementType.MoveForward, 0.05m);
                        if (Debug) { Console.WriteLine($"Moving to {nearestObstacle.ID}; Distance 4"); }
                    }

                    //if (clientBot.Position.DistanceTo(nearestObstacle.Position) > 5)
                    //{
                    //    var angle = clientBot.Position.AngleTo(nearestObstacle.Position);
                    //    angle = (angle + 180) % 360;
                    //    if (clientBot.Heading != angle)  
                    //    { 
                    //        response.SetMovement(MovementType.Turn, angle);
                    //        if (Debug) { Console.WriteLine($"Turning to {nearestObstacle.ID}; Angle {angle}"); }
                    //    } 
                    //    else 
                    //    {
                    //        response.SetMovement(MovementType.MoveForward, 4);
                    //        if (Debug) { Console.WriteLine($"Moving to {nearestObstacle.ID}; Distance 4"); }
                    //    }
                    //}

                    response.SetWeapon(WeaponType.Mortar, 
                        clientBot.Position.AngleTo(nearestObstacle.Position),
                        clientBot.Position.DistanceTo(nearestObstacle.Position));


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
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.Write($"[{DateTime.Now}]");
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine(e.Content);
        }

        private void Socket_OnMessageSend(object sender, SocketMessageEventArgs e)
        {
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.Write($"[{DateTime.Now}]");
            //Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine(e.Content);
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
                Art.Circle((float)o.Position.X, (float)o.Position.Y, 0.5f);
                Art.Fill(PColor.Black);
                Art.Text(o.ID, (float)o.Position.X, (float)o.Position.Y);
            });

            if (Client.Arena.ClientBot is object)
            {
                Art.Fill(PColor.White);
                Art.Circle((float)Client.Arena.ClientBot.Position.X, (float)Client.Arena.ClientBot.Position.Y, 0.5f);
                Art.Fill(PColor.Black);
                Art.Text("BOT\n" + Client.Arena.ClientBot.ID, (float)Client.Arena.ClientBot.Position.X, (float)Client.Arena.ClientBot.Position.Y);
            }
        }

        private async Task RunPings()
        {
            while (Client?.Socket is object)
            {
                Title($"Latency: {await Client.Socket.GetLatency()} ms; FPS: " + FrameRateCurrent + "; Turn: " + Client.Arena.Turn);
                await Task.Delay(500);
            }
        }
    }
}
