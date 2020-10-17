using System;
using System.Linq;
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
        public readonly EventVersion Version;
        public readonly NonEmptySet<PlayerRole> PlayerRoles;
        public readonly NonEmptySet<Card> Cards;
        public readonly Round? ActiveRound;
        public readonly Set<CompletedRound> CompletedRounds;

        public Game(GameId id, EventVersion version, NonEmptySet<PlayerRole> playerRoles, NonEmptySet<Card> cards, Round? activeRound, Set<CompletedRound> completedRounds)
        {
            Id = id;
            Version = version;
            PlayerRoles = playerRoles;
            Cards = cards;
            ActiveRound = activeRound;
            CompletedRounds = completedRounds;
        }

        public UserId GetAdminId()
        {
            return PlayerRoles.Value
                .Where(_ => _.Role == Role.Admin)
                .Single().PlayerId;
        }

        public bool HasPlayer(UserId playerId)
        {
            return PlayerRoles.Value
                .Where(_ => _.PlayerId.Equals(playerId))
                .Count() == 1;
        }

        public bool HasCard(CardId cardId)
        {
            return Cards.Value
                .Where(_ => _.Id.Equals(cardId))
                .Count() == 1;
        }

        public bool HasActiveRound()
        {
            return ActiveRound != null;
        }

        public bool HasAllPlayersSelected()
        {
            if (!HasActiveRound())
            {
                return false;
            }

            return !PlayerRoles.Value
                .Select(_ => _.PlayerId)
                .Except(ActiveRound.Value.PlayerCards.Value.Select(_ => _.PlayerId))
                .Any();
        }
    }
}
