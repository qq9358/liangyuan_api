using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egoal.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() <= 0;
        }
    }
}
