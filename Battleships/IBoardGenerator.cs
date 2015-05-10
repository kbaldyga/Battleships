using System.Collections.Generic;
using System.Drawing;

namespace Battleships
{
    /// <summary>
    /// Interface used to generate initial set up of the board
    /// </summary>
    public interface IBoardGenerator
    {

        /// <summary>
        /// Generates the board with ships
        /// </summary>
        /// <param name="size"></param>
        /// <param name="ships"></param>
        /// <returns></returns>
        ICell[,] GenerateBoard(int size, IList<ShipType> ships);

        /// <summary>
        /// Places ship on a board, starting at the initial position place
        /// </summary>
        /// <param name="board"></param>
        /// <param name="shipType"></param>
        /// <param name="direction"></param>
        /// <param name="initialPosition"></param>
        /// <returns>true if could place the ship, false if unsuccesfull</returns>
        bool PlaceShip(ICell[,] board, ShipType shipType, Direction direction, Point initialPosition);
    }
}