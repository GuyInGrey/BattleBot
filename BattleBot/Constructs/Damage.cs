using System;

namespace BattleBot
{
    public class Damage
    {
        public int Type { get; set; }
        public int Value { get; set; }

        public static Damage FromDynamic(dynamic d)
        {
            return new Damage()
            {
                Type = d.type,
                Value = d.value,
            };
        }
    }
}
