using System.Dynamic;

namespace BattleBot
{
    public class TurnResponse
    {
        private int MoveType;
        private decimal MoveValue;

        private int WeaponTypeInt;
        private decimal WeaponBearing;
        private decimal WeaponRange;

        private decimal ScanValue;

        public void SetMovement(MovementType type, decimal amount)
        {
            MoveType = (int)type;
            MoveValue = amount;
        }

        public void SetWeapon(WeaponType type, decimal bearing, decimal range)
        {
            WeaponTypeInt = (int)type;
            WeaponBearing = bearing;
            WeaponRange = range;
        }

        public void SetScanner(decimal bearing)
        {
            ScanValue = bearing;
        }

        public dynamic GetObject(int turn)
        {
            dynamic toReturn = new ExpandoObject();
            toReturn.turn = turn;

            dynamic move = new ExpandoObject();
            move.type = MoveType;
            move.value = MoveValue;
            toReturn.move = move;

            dynamic weapon = new ExpandoObject();
            weapon.type = WeaponTypeInt;
            weapon.bearing = WeaponBearing;
            weapon.range = WeaponRange;
            toReturn.weapon = weapon;

            toReturn.scan = ScanValue;

            return toReturn;
        }
    }

    public enum MovementType
    {
        None = 0,
        MoveForward = 1,
        Turn = 2,
    }

    public enum WeaponType
    {
        None = 0,
        Cannon = 1,
        Scattergun = 2,
        Mortar = 4,
    }
}
