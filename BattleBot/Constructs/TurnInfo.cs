using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Helpers;

namespace BattleBot
{
    public class TurnInfo
    {
        public Damage[] DamageTaken { get; set; }
        public decimal[] Shots { get; set; }
        public WorldObject[] DestroyedBots { get; set; }
        public string[] DestroyedObjects { get; set; }
        public ScanResult[] ScanResults { get; set; }
        public decimal[] ScannedBy { get; set; }

        public static TurnInfo FromDynamic(dynamic d)
        {
            var toReturn = new TurnInfo();

            var damages = new List<Damage>();
            foreach (var damage in d.damage) { damages.Add(Damage.FromDynamic(damage)); }

            var destroyedBots = new List<WorldObject>();
            foreach (var destroyed in d.botsDestroyed) { destroyedBots.Add(WorldObject.FromDynamic(destroyed)); }

            var scanResults = new List<ScanResult>();
            foreach (var scan in d.scanResults) { scanResults.Add(ScanResult.FromDynamic(scan)); }

            toReturn.DamageTaken = damages.ToArray();
            toReturn.Shots = ((DynamicJsonArray)d.shots).ToList().ConvertAll(o => decimal.Parse(o.ToString())).ToArray();
            toReturn.DestroyedBots = destroyedBots.ToArray();
            toReturn.DestroyedObjects = ((DynamicJsonArray)d.objectsDestroyed).ToList().ConvertAll(o => o.ToString()).ToArray();
            toReturn.ScanResults = scanResults.ToArray();
            toReturn.ScannedBy = ((DynamicJsonArray)d.scannedBy).ToList().ConvertAll(o => decimal.Parse(o.ToString())).ToArray();

            return toReturn;
        }
    }
}
