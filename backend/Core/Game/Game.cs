using System;
using Core.Shared;
using Core.User;

namespace Core.Game
{
    public struct GameId
    {
        public readonly Guid Value;

        public GameId(Guid value)
        {
            Value = value;
        }
    }

    public struct Game
    {
        public readonly GameId Id;
        public readonly GameEventVersion Version;
        public readonly UserId AdminId;
        public readonly NonEmptySet<UserId> PlayerIds;
        public readonly NonEmptySet<Card> Cards;
        public readonly Round? ActiveRound;
        public readonly Set<CompletedRound> CompletedRounds;

        public Game(GameId id, GameEventVersion version, UserId adminId, NonEmptySet<UserId> playerIds, NonEmptySet<Card> cards, Round? activeRound, Set<CompletedRound> completedRounds)
        {
            Id = id;
            Version = version;
            AdminId = adminId;
            PlayerIds = playerIds;
            Cards = cards;
            ActiveRound = activeRound;
            CompletedRounds = completedRounds;
        }
    }
}
