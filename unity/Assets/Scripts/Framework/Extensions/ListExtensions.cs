using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Extensions
{
    public static class ListExtensions
    {
        public static T FirstOfType<T>(this IEnumerable<object> source)
        {
            return source.OfType<T>().FirstOrDefault();
        }

        private static readonly Random _rng = new Random();

        public static T GetRandom<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("Cannot get a random element from an empty or null list.");
            
            int index = _rng.Next(list.Count);
            return list[index];
        }
    }
}