namespace BattleBot
{
    public class ScanResult
    {
        public string Name { get; set; }
        public decimal Bearing { get; set; }
        public decimal R { get; set; }

        public static ScanResult FromDynamic(dynamic d)
        {
            return new ScanResult()
            {
                Name = d.name,
                Bearing = d.bearing,
                R = d.r,
            };
        }
    }
}
