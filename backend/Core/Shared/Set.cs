using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Shared
{
    public class DuplicateException : Exception
    { }

    public class Set<T>
    {
        public readonly IList<T> Value;

        public Set(IList<T> value)
        {
            if (value.Count != value.Distinct().Count())
            {
                throw new DuplicateException();
            }

            Value = value;
        }

        public Set<T> ConcatOne(T item)
        {
            return new Set<T>(
                Value.Concat(new T[] { item }).ToList()
            );
        }

        public override bool Equals(object? obj)
        {
            return obj is Set<T> set &&
                Value.SequenceEqual(set.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
