using System;
using Core.Shared;

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
        public readonly Lobby Lobby;
        public readonly Round? ActiveRound;
        public readonly Set<CompletedRound> CompletedRounds;

        public Game(Lobby lobby, Round? activeRound, Set<CompletedRound> completedRounds)
        {
            Lobby = lobby;
            ActiveRound = activeRound;
            CompletedRounds = completedRounds;
        }
    }
}
