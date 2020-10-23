using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.GameDomain
{
    public class CompletedRoundSet
    {
        public class DuplicateRoundException : Exception { }

        public readonly IList<CompletedRound> Value;

        public CompletedRoundSet(params CompletedRound[] value)
        {
            if (value.Select(_ => _.Id).Distinct().Count() != value.Count())
            {
                throw new DuplicateRoundException();
            }

            Value = value;
        }

        public CompletedRoundSet AddRound(CompletedRound round)
        {
            return new CompletedRoundSet(Value.ConcatOne(round).ToArray());
        }

        public override bool Equals(object? obj)
        {
            return obj is CompletedRoundSet set &&
                   Value.SequenceEqual(set.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
