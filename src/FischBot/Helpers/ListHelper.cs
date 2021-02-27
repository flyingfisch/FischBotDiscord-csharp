using System;
using System.Collections.Generic;
using System.Linq;

namespace FischBot.Helpers
{
    public static class ListHelper
    {
        /// <summary>
        /// Helper method that shuffles the contents of a collection randomly.
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="collection">The collection to shuffle</param>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            var rand = new Random();

            return collection.OrderBy(x => rand.Next());
        }
    }
}