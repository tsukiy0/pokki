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

    public enum GameStatus
    {
        Pending,
        Active,
        Inactive
    }

    public struct Game
    {
        public readonly GameId Id;
        public readonly PlayerRoleSet PlayerRoles;
        public readonly CardSet Cards;
        public readonly Round? ActiveRound;
        public readonly CompletedRoundSet CompletedRounds;
        public readonly GameStatus Status;

        public Game(GameId id, GameStatus status, PlayerRoleSet playerRoles, CardSet cards, Round? activeRound, CompletedRoundSet completedRounds)
        {
            Id = id;
            Status = status;
            PlayerRoles = playerRoles;
            Cards = cards;
            ActiveRound = activeRound;
            CompletedRounds = completedRounds;
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
    }
}
