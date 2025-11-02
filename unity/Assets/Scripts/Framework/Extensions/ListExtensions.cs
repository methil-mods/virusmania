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
    }
}