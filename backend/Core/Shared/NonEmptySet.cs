using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Shared
{
    public class NotEmptyException : Exception
    { }

    public class NonEmptySet<T> : Set<T>
    {
        public NonEmptySet(IList<T> value) : base(value)
        {
            if (value.Count == 0)
            {
                throw new NotEmptyException();
            }
        }

        public new NonEmptySet<T> ConcatOne(T item)
        {
            return new NonEmptySet<T>(
                Value.Concat(new T[] { item }).ToList()
            );
        }
    }
}
