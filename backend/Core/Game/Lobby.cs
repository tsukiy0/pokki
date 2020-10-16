using Core.User;
using Core.Shared;
using System;

namespace Core.Game
{

    public struct Lobby
    {
        public readonly GameId Id;
        public readonly UserId AdminId;
        public readonly NonEmptySet<UserId> PlayerIds;
        public readonly NonEmptySet<Card> Cards;

        public Lobby(GameId id, UserId adminId, NonEmptySet<UserId> playerIds, NonEmptySet<Card> cards)
        {
            Id = id;
            AdminId = adminId;
            PlayerIds = playerIds;
            Cards = cards;
        }
    }
}
