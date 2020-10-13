using System.Collections.Generic;

namespace BattleBot
{
    public class ServerArena
    {
        /// <summary>
        /// Key is unique Socket ID
        /// </summary>
        public Dictionary<string, Bot> Players = new Dictionary<string, Bot>();
        public List<WorldObject> Obstacles = new List<WorldObject>();
        public double Size;
    }
}
