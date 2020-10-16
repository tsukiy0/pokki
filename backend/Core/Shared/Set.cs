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
    }
}
