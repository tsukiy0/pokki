using Core.Shared;
using Core.UserDomain;
using System;
using System.Linq;

namespace Core.GameDomain
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
        public readonly PlayerRoleSet PlayerRoles;
        public readonly CardSet Cards;
        public readonly Round? ActiveRound;
        public readonly Set<CompletedRound> CompletedRounds;

        public Game(GameId id, EventVersion version, PlayerRoleSet playerRoles, CardSet cards, Round? activeRound, Set<CompletedRound> completedRounds)
        {
            Id = id;
            Version = version;
            PlayerRoles = playerRoles;
            Cards = cards;
            ActiveRound = activeRound;
            CompletedRounds = completedRounds;
        }

        public bool IsActiveRound()
        {
            return ActiveRound != null;
        }

        public bool HasAllPlayersSelected()
        {
            if (ActiveRound == null)
            {
                return false;
            }

            return !PlayerRoles.Value
                .Select(_ => _.PlayerId)
                .Except(ActiveRound.Value.PlayerCards.Value.Select(_ => _.PlayerId))
                .Any();
        }

        public bool IsNextVersion(EventVersion version)
        {
            return version.Value == Version.Value + 1;
        }
    }
}
