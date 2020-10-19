using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class ListExtensions
    {
        public static IList<T> ConcatOne<T>(this IList<T> list, T item)
        {
            return list.Concat(new T[] { item }).ToArray();
        }
    }
}
