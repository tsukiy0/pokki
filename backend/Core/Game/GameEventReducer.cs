using System;
using System.Linq;
using Core.Shared;
using Core.User;

namespace Core.Game
{
    public class NoNewGameException : Exception { }
    public class MultipleNewGameException : Exception { }
    public class EventOrderException : Exception { }

    public class GameEventReducer
    {
        public Game Reduce(NonEmptySet<GameEvent> events)
        {
            if (events.Value.First() is NewGameEvent newGameEvent)
            {
                return new Game(
                    newGameEvent.GameId,
                    newGameEvent.Version,
                    newGameEvent.AdminId,
                    new NonEmptySet<UserId>(new UserId[] { newGameEvent.AdminId }),
                    newGameEvent.Cards,
                    null,
                    new Set<CompletedRound>(Array.Empty<CompletedRound>())
                );
            }
            else
            {
                throw new NoNewGameException();
            }
        }
    }
}