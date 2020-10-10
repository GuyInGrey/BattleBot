namespace BattleBot
{
    public class Bot : WorldObject
    {
        public string Name { get; set; }
        public double Heading { get; set; }
        public double Scanner { get; set; }

        public int TurnsAgoSeen { get; set; }
    }
}