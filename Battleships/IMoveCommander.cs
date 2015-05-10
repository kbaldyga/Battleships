using System.Drawing;

namespace Battleships
{
    public interface IMoveCommander
    {
        /// <summary>
        /// Indicates the next move
        /// </summary>
        /// <returns></returns>
        Point GetMove();
    }
}