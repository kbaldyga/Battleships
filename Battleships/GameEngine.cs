using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Battleships
{
    public class GameEngine
    {
        private readonly ICell[][,] gameState;
        private readonly IBoardGenerator generator;
        private readonly int size;

        public GameEngine(int size, IBoardGenerator boardGenerator)
        {
            this.size = size;
            generator = boardGenerator;
            gameState = new ICell[2][,];
        }

        /// <summary>
        ///     Initializes the game engine. Needs to be called before the game starts.
        /// </summary>
        /// <param name="ships"></param>
        public void Initialize(IList<ShipType> ships)
        {
            for (var i = 0; i < gameState.GetLength(0); i ++)
            {
                gameState[i] = generator.GenerateBoard(size, ships);
            }
        }

        /// <summary>
        ///     Executes the move.
        /// </summary>
        /// <param name="point">Where to hit</param>
        /// <param name="playerNumber">Current player</param>
        /// <returns>a tuple describing the move outcome, including the ship type, if ship has been sunk with the last hit</returns>
        public Tuple<MoveResult, ShipType> DoMove(Point point, int playerNumber)
        {
            // gets the board for the current player
            var currentBoard = gameState[playerNumber];
            if (!ValidateMove(point, currentBoard))
            {
                return Tuple.Create(MoveResult.InvalidMove, ShipType.None);
            }

            var currentCell = currentBoard[point.Y, point.X] as ShipCell;
            // If cell is not a ship, then it must be empty
            if (currentCell == null)
            {
                return Tuple.Create(MoveResult.Miss, ShipType.None);
            }
            currentCell.Hit = true;
            if (currentCell.WholeShip.All(s => s.Hit))
            {
                return Tuple.Create(IsGameFinished(currentBoard) ? MoveResult.PlayerWon : MoveResult.HitAndDrown,
                    currentCell.Type);
            }
            return Tuple.Create(MoveResult.Hit, ShipType.None);
        }

        private static bool IsGameFinished(ICell[,] currentBoard)
        {
            return currentBoard.Flatten().OfType<ShipCell>().All(s => s.Hit);
        }

        /// <summary>
        ///     Valid move is: within the bounds of the board and the cell is either empty or a ship that has not yet been hit
        /// </summary>
        /// <returns><c>true</c>, if move was validated, <c>false</c> otherwise.</returns>
        /// <param name="point">Point.</param>
        /// <param name="currentBoard">current board</param>
        private bool ValidateMove(Point point, ICell[,] currentBoard)
        {
            var outOfBounds = point.X >= 0 && point.Y >= 0 &&
                              point.X < size && point.Y < size;
            if (!outOfBounds)
                return false;
            var currentState = currentBoard[point.Y, point.X] as ShipCell;
            // move is valid if an empty cell
            if (currentState == null)
                return true;
            // or a ship, but not yet hit
            return !currentState.Hit;
        }

        public override string ToString()
        {
            Action<ICell[,], int, StringBuilder> generateRow = (board, row, currentSb) =>
            {
                for (var j = 0; j < size; j++)
                {
                    var shipState = board[row, j] as ShipCell;
                    if (shipState == null)
                    {
                        currentSb.Append("O ");
                    }
                    else
                    {
                        currentSb.Append(shipState.Hit ? "X " : shipState.Type.ShipSize() + " ");
                    }
                }
            };

            var sb = new StringBuilder();
            sb.AppendLine("   a b c d e f g h i j ").AppendLine();

            for (var i = 0; i < size; i++)
            {
                sb.AppendFormat("{0:00} ", (i + 1));
                generateRow(gameState[0], i, sb);
                sb.Append(" ");
                generateRow(gameState[1], i, sb);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}