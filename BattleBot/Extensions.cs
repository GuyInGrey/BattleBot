using Newtonsoft.Json;

namespace BattleBot
{
    public static class Extensions
    {
        public static dynamic FromJson(this string json) =>
            JsonConvert.DeserializeObject<dynamic>(json);

        public static string ToJson(dynamic obj) =>
            JsonConvert.SerializeObject(obj);
    }
}
