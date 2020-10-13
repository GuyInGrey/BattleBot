namespace BattleBot
{
    public class Bot : WorldObject
    {
        public string Name { get; set; }
        public decimal Heading { get; set; }
        public decimal Scanner { get; set; }

        public int TurnsAgoSeen { get; set; }

        public static Bot FromDynamic(dynamic d)
        {
            return new Bot()
            {
                ID = d.id,
                Health = d.hp,
                X = d.x,
                Y = d.y,
                R = d.r,
                Name = d.name,
                Heading = d.heading,
                Scanner = d.scanner,
            };
        }
    }
}
