using System;

namespace BattleBot
{
    public struct Vector2
    {
        public decimal X;
        public decimal Y;

        public Vector2(decimal x, decimal y) { X = x; Y = y; }

        public decimal DistanceTo(Vector2 pos2) => (decimal)Math.Sqrt((double)DistanceToSquared(pos2));
        public decimal DistanceToSquared(Vector2 pos2) => (decimal)(Math.Pow((double)(pos2.X - X), 2) + Math.Pow((double)(pos2.Y - Y), 2));

        public decimal AngleTo(Vector2 pos2)
        {
            var xDiff = pos2.X - X;
            var yDiff = -(pos2.Y - Y);
            return (decimal)(Math.Atan2((double)yDiff, (double)xDiff) * 180.0 / Math.PI) + 90m;
        }

        public override string ToString() => $"({X},{Y})";
    }
}
