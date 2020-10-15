using System;

namespace BattleBot
{
    public class JoinArenaInfo
    {
        public string Room;
        public string Password;
        public string ClientName;
        public int BattleCount;
        public int Team;
        public int RoomCapacity;
        public DateTime StartTime;

        public dynamic ToDynamic() =>
            (dynamic)new
            {
                room = Room,
                pwd = Password,
                name = ClientName,
                battles = BattleCount,
                team = Team,
                capacity = RoomCapacity,
                start = StartTime,
            };
    }
}
