using System;
using System.Collections.Generic;
using System.Drawing;

namespace Battleships
{
    /// <summary>
    /// Implementation of IBoardGenerator, uses System.Random to place ships on the board
    /// </summary>
    public class RandomBoardGenerator : IBoardGenerator
    {
        private readonly Random randomizer;

        public RandomBoardGenerator()
        {
            randomizer = new Random();
        }

        public ICell[,] GenerateBoard(int size, IList<ShipType> ships)
        {
            // generate an empty board ...
            var board = new ICell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = new EmptyCell();
                }
            }

            // ...and place ships on it
            foreach (var ship in ships)
            {
                bool placedShip;
                do
                {
                    var direction = (Direction) (randomizer.Next()%2);
                    var a = randomizer.Next(0, size - ship.ShipSize());
                    var b = randomizer.Next(0, size);
                    var initialPosition = direction == Direction.Horizontal ? new Point(a, b) : new Point(b, a);
                    placedShip = PlaceShip(board, ship, direction, initialPosition);
                } while (!placedShip);
            }

            return board;
        }


        public bool PlaceShip(ICell[,] board, ShipType shipType, Direction direction, Point initialPosition)
        {
            // initial position placed outside of the board
            if (initialPosition.X >= board.GetLength(0) || initialPosition.Y >= board.GetLength(0)) return false;

            var significantIndex = direction == Direction.Horizontal ? initialPosition.X : initialPosition.Y;
            Func<int, Point> getBoardPosition = ix => direction == Direction.Horizontal ? new Point(ix, initialPosition.Y) : new Point(initialPosition.X, ix);

            // position that would make the ship "stick" out of the board
            if (significantIndex + shipType.ShipSize() >= board.GetLength(0)) return false;
            
            // positions that are already taken (ships can not cross)
            for (int i = significantIndex; i < significantIndex + shipType.ShipSize(); i++)
            {
                var boardPosition = getBoardPosition(i);
                if (!(board[boardPosition.Y, boardPosition.X] is EmptyCell)) return false;
            }

            // valid position, placing the ship
            var wholeShip = new List<ShipCell>();
            for (int i = significantIndex; i < significantIndex + shipType.ShipSize(); i++)
            {
                var boardPosition = getBoardPosition(i);
                var shipCell = new ShipCell(shipType, wholeShip);
                wholeShip.Add(shipCell);
                board[boardPosition.Y, boardPosition.X] = shipCell;
            }

            return true;
        }
    }
}