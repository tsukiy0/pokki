using System;
using System.Collections.Generic;

namespace Core
{
    public struct RoundId
    {
        public readonly Guid Value;

        public RoundId(Guid value)
        {
            Value = value;
        }
    }

    public struct PersonCard
    {
        public readonly PersonId PersonId;
        public readonly CardId CardId;

        public PersonCard(PersonId personId, CardId cardId)
        {
            PersonId = personId;
            CardId = cardId;
        }
    }

    public struct Round
    {
        public readonly RoundId Id;
        public readonly string Name;
        public readonly IList<PersonCard> PersonCards;

        public Round(RoundId id, string name, IList<PersonCard> personCards)
        {
            Id = id;
            Name = name;
            PersonCards = personCards;
        }
    }
}
