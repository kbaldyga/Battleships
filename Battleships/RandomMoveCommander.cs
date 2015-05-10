using System;
using System.Collections.Generic;
using System.Drawing;

namespace Battleships
{
    /// <summary>
    /// Implementation of IMoveCommander which uses System.Random to generate the next move
    /// </summary>
    public class RandomMoveCommander : IMoveCommander
    {
        private readonly Random random;
        private readonly int size;
        private readonly HashSet<Point> previousMoves; 

        public RandomMoveCommander(int size)
        {
            this.size = size;
            random = new Random();
            previousMoves = new HashSet<Point>();
        }

        public Point GetMove()
        {
            while (true)
            {
                var newMove = new Point(random.Next(size), random.Next(size));
                if (previousMoves.Contains(newMove))
                {
                    continue;
                }
                previousMoves.Add(newMove);
                return newMove;
            }
        }
    }
}