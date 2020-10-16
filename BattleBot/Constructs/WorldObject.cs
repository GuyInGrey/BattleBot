namespace BattleBot
{
    public class WorldObject
    {
        public string ID { get; set; }
        public decimal Health { get; set; }
        public decimal Radius { get; set; }
        public Vector2 Position { get; set; }

        public static WorldObject FromDynamic(dynamic d)
        {
            return new WorldObject()
            {
                ID = d.id,
                Health = d.hp,
                Position = new Vector2(d.x, d.y),
                Radius = d.r,
            };
        }
    }
}
