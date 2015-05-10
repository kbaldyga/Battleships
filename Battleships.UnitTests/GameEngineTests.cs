using System.Collections.Generic;
using System.Drawing;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace Battleships.UnitTests
{
    [TestFixture]
    public class GameEngineTests
    {
        private GameEngine gameEngine;
        private Mock<IBoardGenerator> mockBoardGenerator;
        private ICell[,] currentBoard;

        [SetUp]
        public void SetUp()
        {
            var ships = new List<ShipType>
            {
                ShipType.Battleship,
                ShipType.Destroyer
            };
            currentBoard = generateBoard(10, ships);
            mockBoardGenerator = new Mock<IBoardGenerator>();
            mockBoardGenerator.Setup(s => s.GenerateBoard(10, ships))
                .Returns(currentBoard);

            gameEngine = new GameEngine(10, mockBoardGenerator.Object);
            gameEngine.Initialize(ships);
        }

        [Test]
        [TestCase(11, 0)]
        [TestCase(0, 11)]
        public void DoMove_WhenMoveOutOfBoundsOfBoard_ReturnsInvalidMove(int x, int y)
        {
            gameEngine.DoMove(new Point(x, y), 0).Item1
                .ShouldBe(MoveResult.InvalidMove);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        public void DoMove_WhenShipAlreadyHit_ReturnsInvalidMove(int x, int y)
        {
            gameEngine.DoMove(new Point(x, y), 0).Item1
               .ShouldBe(MoveResult.Hit);

            gameEngine.DoMove(new Point(x, y), 0).Item1
                .ShouldBe(MoveResult.InvalidMove);
        }

        [Test]
        [TestCase(9, 9)]
        [TestCase(8, 8)]
        public void DoMove_WhenHitEmptyCell_ReturnsMiss(int x, int y)
        {
            gameEngine.DoMove(new Point(x, y), 0).Item1
                .ShouldBe(MoveResult.Miss);
        }

        [Test]
        public void DoMove_WhenWholeShipHit_ReturnsHitAndDrown()
        {
            gameEngine.DoMove(new Point(0, 0), 0).Item1
                .ShouldBe(MoveResult.Hit);
            gameEngine.DoMove(new Point(1, 0), 0).Item1
                .ShouldBe(MoveResult.Hit);
            gameEngine.DoMove(new Point(2, 0), 0).Item1
                .ShouldBe(MoveResult.Hit);
            gameEngine.DoMove(new Point(3, 0), 0).Item1
                .ShouldBe(MoveResult.Hit);
            var move = gameEngine.DoMove(new Point(4, 0), 0);
            move.Item1.ShouldBe(MoveResult.HitAndDrown);
            move.Item2.ShouldBe(ShipType.Battleship);
        }

        [Test]
        public void DoMove_WhenAllShipsDown_ReturnsGameFinished()
        {
            for (int i = 0; i < currentBoard.GetLength(0); i++)
            {
                currentBoard[0, i] = new EmptyCell();
            }

            gameEngine.DoMove(new Point(0, 1), 0).Item1
                .ShouldBe(MoveResult.Hit);
            gameEngine.DoMove(new Point(1, 1), 0).Item1
                .ShouldBe(MoveResult.Hit);
            gameEngine.DoMove(new Point(2, 1), 0).Item1
                .ShouldBe(MoveResult.Hit);
            gameEngine.DoMove(new Point(3, 1), 0).Item1
                .ShouldBe(MoveResult.PlayerWon);
        }

        private static ICell[,] generateBoard(int size, IList<ShipType> ships)
        {
            var board = new ICell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = new EmptyCell();
                }
            }
            for (int i = 0; i < ships.Count; i++)
            {
                var wholeShip = new List<ShipCell>();
                for (int j = 0; j < ships[i].ShipSize(); j++)
                {
                    var shipCell = new ShipCell(ships[i], wholeShip);
                    wholeShip.Add(shipCell);
                    board[i, j] = shipCell;
                }
            }

            return board;
        }
        
    }
}
