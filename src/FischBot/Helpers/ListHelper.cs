using System;
using System.Collections.Generic;

namespace FischBot.Helpers
{
    public static class ListHelper
    {
        /// <summary>
        /// Helper method that shuffles the contents of a list randomly.
        /// </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="list">List of items</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            var rand = new Random();

            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}