using Core.Shared;
using Core.UserDomain;
using System;

namespace Core.GameDomain
{
    public struct RoundId
    {
        public readonly Guid Value;

        public RoundId(Guid value)
        {
            Value = value;
        }
    }

    public struct PlayerCard
    {
        public readonly UserId PlayerId;
        public readonly CardId CardId;

        public PlayerCard(UserId userId, CardId cardId)
        {
            PlayerId = userId;
            CardId = cardId;
        }
    }

    public struct Round
    {
        public readonly RoundId Id;
        public readonly string Name;
        public readonly Set<PlayerCard> PlayerCards;

        public Round(RoundId id, string name, Set<PlayerCard> playerCards)
        {
            Id = id;
            Name = name;
            PlayerCards = playerCards;
        }
    }

    public struct CompletedRound
    {
        public readonly RoundId Id;
        public readonly string Name;
        public readonly NonEmptySet<PlayerCard> PlayerCards;
        public readonly CardId ResultCardId;

        public CompletedRound(RoundId id, string name, NonEmptySet<PlayerCard> playerCards, CardId resultCardId)
        {
            Id = id;
            Name = name;
            PlayerCards = playerCards;
            ResultCardId = resultCardId;
        }
    }
}
