using System.Collections.Generic;

namespace BattleBot
{
    public class ClientArena
    {
        public double Size { get; set; }
        public double Capacity { get; set; }
        public List<WorldObject> Obstacles { get; set; } = new List<WorldObject>();
        public List<Bot> Bots { get; set; } = new List<Bot>();
        public Bot ClientBot;

        public int Turn { get; set; }
        public int NextTurn { get; set; }
        public int PlayersRemaining { get; set; }

    }
}
