using System;
using System.Threading.Tasks;

namespace BattleBot.Tests
{
    class Program
    {
        static void Main()
        {
            var o = new WorldObject() { Position = new Vector2(5, 3) };
            var o2 = new WorldObject() { Position = new Vector2(6, 4) };

            Console.WriteLine(default(Vector2));

            Console.WriteLine(o.Position.AngleTo(o2.Position));

            Task.Run(() => new Window());
            new Window();
            Console.Read();
        }
    }
}
