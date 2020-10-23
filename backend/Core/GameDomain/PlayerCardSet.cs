using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.GameDomain
{
    public class PlayerCardSet
    {
        public class DuplicatePlayerException : Exception { }

        public IList<PlayerCard> Value;

        public PlayerCardSet(params PlayerCard[] value)
        {
            if (value.Select(_ => _.PlayerId).Distinct().Count() != value.Count())
            {
                throw new DuplicatePlayerException();
            }

            Value = value;
        }

        public PlayerCardSet AddPlayerCard(PlayerCard playerCard)
        {
            return new PlayerCardSet(Value.ConcatOne(playerCard).ToArray());
        }

        public override bool Equals(object? obj)
        {
            return obj is PlayerCardSet set &&
                   Value.SequenceEqual(set.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
