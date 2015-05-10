using System.Collections.Generic;

namespace Battleships
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Copied from stackoverflow, flattens two-dimensional array, into a stream of ienumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }
    }
}