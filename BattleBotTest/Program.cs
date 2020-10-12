using System;
using BattleBot;
using Newtonsoft.Json;

namespace BattleBot.Tests
{
    class Program
    {
        static void Main()
        {
            var server = new BattleBotServer("ws://localhost:3000");

            var client = new BattleBotClient();
            client.OnError = (error, details) =>
            {
                Console.WriteLine(error + "\n" + JsonConvert.DeserializeObject(details));
            };
            client.OnReady = () =>
            {
                Console.WriteLine("Ready! Token: " + client.Token);
            };
            client.OnGameEnd = (winner, rounds) =>
            {
                Console.WriteLine($"{winner} has won the game! ({rounds} rounds)");
            };
            client.OnTurn = (turnInfo) =>
            {

            };
            client.Start(@"ws://localhost:3000");

            Console.Read();
        }
    }
}
