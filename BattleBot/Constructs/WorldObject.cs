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
        public double Health { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double R { get; set; }
    }
}
