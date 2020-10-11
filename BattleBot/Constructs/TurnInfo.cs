using System.Collections.Generic;

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
            toReturn.Shots = d.shots;
            toReturn.DestroyedBots = destroyedBots.ToArray();
            toReturn.DestroyedObjects = d.objectsDestroyed;
            toReturn.ScanResults = scanResults.ToArray();
            toReturn.ScannedBy = d.scannedBy;

            return toReturn;
        }
    }
}
