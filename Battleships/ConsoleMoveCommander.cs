using System;
using System.Drawing;
using System.Linq;

namespace Battleships
{
    /// <summary>
    /// Implementation of IMoveCommander which uses Console.ReadLine to get the next move (format: CharacterNumber)
    /// </summary>
    public class ConsoleMoveCommander : IMoveCommander
    {
        public Point GetMove()
        {
            Console.WriteLine("Enter next move (e.g. A10)");
            try
            {
                var line = Console.ReadLine().ToUpperInvariant();
                var character = line[0] - 'A';
                var number = int.Parse(new string(line.Skip(1).ToArray()))-1;   // zero-based index, hence -1
                return new Point(character, number);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not understand the direction. Try again");
                return GetMove();
            }
        }
    }
}