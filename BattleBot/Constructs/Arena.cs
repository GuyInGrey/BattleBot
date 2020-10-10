using System.Collections.Generic;

namespace BattleBot
{
    public class Arena
    {
        public double Size { get; set; }
        public double Capacity { get; set; }
        public List<WorldObject> Obstacles { get; set; } = new List<WorldObject>();
        public List<Bot> Bots { get; set; }
    }
}
