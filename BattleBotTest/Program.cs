using System;
using System.Linq;
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

            Client = new BattleBotClient();
            Client.OnError = (error, details) =>
            {
                Console.WriteLine(error + "\n" + JsonConvert.DeserializeObject(details));
            };
            Client.OnReady = () =>
            {
                Console.WriteLine("Ready! Token: " + Client.Token);
            };
            Client.OnGameEnd = (winner, rounds) =>
            {
                Console.WriteLine($"{winner} has won the game! ({rounds} rounds)");
            };
            Client.OnTurn = (turnInfo) =>
            {

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
