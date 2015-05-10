using System;
using System.Collections.Generic;

namespace Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            const int size = 10;
            
            var boardGenerator = new RandomBoardGenerator();
            var engine = new GameEngine(size, boardGenerator);
            engine.Initialize(new List<ShipType> { ShipType.Destroyer, ShipType.Battleship, ShipType.Destroyer });
            var moveCommander = new IMoveCommander[] {new ConsoleMoveCommander(), new RandomMoveCommander(size)};

            var keepGoing = true;
            
            while (keepGoing)
            {
                Console.WriteLine(engine);
                for (var i = 0; i < 2; i++)
                {
                    Tuple<MoveResult, ShipType> moveResult;
                    do
                    {
                        var move = moveCommander[i].GetMove();
                        moveResult = engine.DoMove(move, i);

                        if (moveResult.Item1 == MoveResult.PlayerWon)
                        {
                            keepGoing = false;
                            Console.WriteLine("Player {0} won!", i);
                            break;
                        }
                        if (moveResult.Item1 == MoveResult.HitAndDrown)
                        {
                            Console.WriteLine(moveResult.Item2 + " is down.");
                        }
                        else
                        {
                            Console.WriteLine("Player {0} {1}", i, moveResult.Item1);
                        }

                    } while (moveResult.Item1 == MoveResult.InvalidMove);
                }
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
