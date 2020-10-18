using System;
using System.Threading.Tasks;

namespace BattleBot.Tests
{
    class Program
    {
        static void Main()
        {
            var p1 = new Vector2(0, 0);
            var p2 = new Vector2(0, 1);

            Console.WriteLine(p1.AngleTo(p2));
            p2 = Vector2.RotatePoint(p2, p1, 90);
            Console.WriteLine(p1.AngleTo(p2));

            Task.Run(() => new Window(false));
            new Window(true);
            Console.Read();
        }
    }
}
