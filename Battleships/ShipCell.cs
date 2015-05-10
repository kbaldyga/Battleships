using System.Collections.Generic;

namespace Battleships
{
    /// <summary>
    /// Cell with a ship
    /// </summary>
    public class ShipCell : ICell
    {
        public ShipType Type { get; private set; }
        /// <summary>
        /// Indicates wheather the ship has been hit
        /// </summary>
        public bool Hit { get; set; }
        
        /// <summary>
        /// "the rest" of the ship
        /// </summary>
        public IList<ShipCell> WholeShip { get; private set; }

        public ShipCell(ShipType type, IList<ShipCell> wholeShip)
        {
            this.Type = type;
            this.WholeShip = wholeShip;
        }
    }
}