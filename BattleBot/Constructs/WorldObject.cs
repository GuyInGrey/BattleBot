using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot
{
    public class WorldObject : IID, IHealth, IPositional
    {
        public string ID { get; set; }
        public decimal Health { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal R { get; set; }

        public static WorldObject FromDynamic(dynamic d)
        {
            return new WorldObject()
            {
                ID = d.id,
                Health = d.hp,
                X = d.x,
                Y = d.y,
                R = d.r,
            };
        }
    }
}
