using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.GameDomain
{
    public class CardSet
    {
        public class DuplicateCardException : Exception { }
        public class DuplicateCardNameException : Exception { }

        public IList<Card> Value;

        public CardSet(params Card[] value)
        {
            if (value.Select(_ => _.Id).Distinct().Count() != value.Count())
            {
                throw new DuplicateCardException();
            }

            if (value.Select(_ => _.Name).Distinct().Count() != value.Count())
            {
                throw new DuplicateCardNameException();
            }

            Value = value;
        }

        public CardSet AddCard(Card card)
        {
            return new CardSet(Value.ConcatOne(card).ToArray());
        }

        public override bool Equals(object? obj)
        {
            return obj is CardSet set &&
                   Value.SequenceEqual(set.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
