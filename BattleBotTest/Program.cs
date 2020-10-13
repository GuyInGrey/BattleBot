using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BattleBot.Tests
{
    class Program
    {
        static BattleBotClient Client;

        static void Main()
        {
            var server = new BattleBotServer("ws://localhost:3000");

            Client = new BattleBotClient
            {
                OnError = (error, details) =>
                {
                    //Console.WriteLine(error + "\n" + JsonConvert.DeserializeObject(details));
                },
                OnReady = () =>
                {
                    //Console.WriteLine("Ready! Token: " + Client.Token);
                },
                OnGameEnd = (winner, rounds) =>
                {
                    //Console.WriteLine($"{winner} has won the game! ({rounds} rounds)");
                },
                OnTurn = (turnInfo, response) =>
                {
                    response.SetMovement(MovementType.MoveForward, 3.5m);
                    response.SetScanner(10m);
                    response.SetWeapon(WeaponType.Mortar, 90m, 2m);
                    return response;
                }
            };
            Client.Start(@"ws://localhost:3000");
            RunPings();

            Console.Read();
        }

        static void RunPings()
        {
            Task.Run(() =>
            {
                Console.Title = "Latency: " + Client.Socket.GetLatency().GetAwaiter().GetResult();
                Thread.Sleep(5000);
                RunPings();
            });
        }
    }
}
