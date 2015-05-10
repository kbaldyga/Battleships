using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace Battleships.UnitTests
{
    [TestFixture]
    public class BoardGeneratorTests
    {
        private IBoardGenerator boardGenerator;

        [SetUp]
        public void SetUp()
        {
            boardGenerator = new RandomBoardGenerator();    
        }

        private ICell[,] GenerateEmptyBoard(int size)
        {
            var board = new ICell[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i,j] = new EmptyCell();
                }
            }
            return board;
        }

        [Test]
        [TestCase(0, 100, Direction.Vertical)]
        [TestCase(100, 0, Direction.Vertical)]
        [TestCase(0, 100, Direction.Horizontal)]
        [TestCase(100, 0, Direction.Horizontal)]
        public void PlaceShip_WhenPlacedOutOfTheBoard_ReturnsFalse(int x, int y, Direction direction)
        {
            var board = GenerateEmptyBoard(10);
            var outOfBounds = new Point(x, y);
            boardGenerator.PlaceShip(board, ShipType.Battleship, direction, outOfBounds)
                .ShouldBe(false);
        }

        [Test]
        [TestCase(6, 0, Direction.Horizontal, 5)]
        [TestCase(0, 6, Direction.Vertical, 5)]
        [TestCase(7, 0, Direction.Horizontal, 4)]
        [TestCase(0, 7, Direction.Vertical, 4)]
        public void PlaceShip_WhenShipTooLong_ReturnsFalse(int x, int y, Direction direction, int shipSize)
        {
            var board = GenerateEmptyBoard(10);
            var initialPoint = new Point(x, y);
            boardGenerator.PlaceShip(board, (ShipType)shipSize, direction, initialPoint)
                .ShouldBe(false);
        }

        [Test]
        [TestCase(2, 0, Direction.Vertical)]
        [TestCase(0, 2, Direction.Horizontal)]
        public void PlaceShip_WhenOneSegmentCrossesWithDifferentShip_ReturnsFalse(int x, int y, Direction direction)
        {
            var board = GenerateEmptyBoard(10);
            board[2, 2] = new ShipCell(ShipType.Destroyer, new List<ShipCell>());

            var initialPosition = new Point(x, y);
            boardGenerator.PlaceShip(board, ShipType.Battleship, direction, initialPosition)
                .ShouldBe(false);
        }

        [Test]
        [TestCase(Direction.Vertical)]
        [TestCase(Direction.Horizontal)]
        public void PlaceShip_WhenCorrectlyPlaces_ShipCellKnowsWholeShip(Direction direction)
        {
            var board = GenerateEmptyBoard(10);
            var initialPosition = new Point(0, 0);
            boardGenerator.PlaceShip(board, ShipType.Battleship, direction, initialPosition)
                .ShouldBe(true);
            board.Flatten().OfType<ShipCell>().Count().ShouldBe(ShipType.Battleship.ShipSize());
            (board[0,0] as ShipCell).WholeShip.Count.ShouldBe(ShipType.Battleship.ShipSize());
        }

        [Test]
        [TestCase(new []{ShipType.Battleship, ShipType.Destroyer})]
        [TestCase(new[] { ShipType.Destroyer, ShipType.Destroyer })]
        public void GenerateBoard_ShouldHaveExpectedNumberOfShips(params ShipType[] ships)
        {
            var expectedShipCells = ships.Sum(s => s.ShipSize());
            var board = boardGenerator.GenerateBoard(10, ships.ToList());
            var flattenedBoard = board.Flatten().OfType<ShipCell>().ToList();
            flattenedBoard.Count().ShouldBe(expectedShipCells);
            foreach (var shipType in ships)
            {
                flattenedBoard.ShouldContain(c => c.Type == shipType);
            }
        }
    }
}
