using System;
using BattleBot;

namespace BattleBot.Tests
{
    class Program
    {
        static void Main()
        {
            var client = new BattleBotClient(@"ws://serverUrlHere");
            client.OnError = (error, details) =>
            {
                Console.WriteLine(error + "\n" + Extensions.ToJson(details));
            };
            client.OnReady = () =>
            {
                Console.WriteLine("Ready!");
            };
            client.OnGameEnd = (winner, rounds) =>
            {
                Console.WriteLine($"{winner} has won the game! ({rounds} rounds)");
            };

            Console.Read();
        }
    }
}
