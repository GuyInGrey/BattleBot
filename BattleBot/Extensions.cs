using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BattleBot
{
    public static class Extensions
    {
        public static WorldObject GetNearest(this IEnumerable<WorldObject> a, WorldObject b)
        {
            if (a.Count() < 1) { return null; }
            return a.OrderBy(obj1 => obj1.Position.DistanceTo(b.Position)).First();
        }
    }
}