namespace Battleships
{
    public static class ShipTypeExtensions
    {
        public static int ShipSize(this ShipType shipType)
        {
            return (int) shipType;
        }
    }
}