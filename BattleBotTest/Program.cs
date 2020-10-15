using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BattleBot.Tests
{
    class Program
    {
        static void Main()
        {
            Task.Run(() => new Window());
            new Window();
            Console.Read();
        }
    }
}
