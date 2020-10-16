using System;
using System.Collections.Generic;

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
    }
}
