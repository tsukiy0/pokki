using System;
using System.Linq;
using Core.Shared;

namespace Core.Game
{
    public class NoNewGameException : Exception { }
    public class MultipleNewException : Exception { }
    public class EventOrderException : Exception { }

    public class EventReducer
    {
        public Game Reduce(NonEmptySet<Event> events)
        {
            if (events.Value.Where(_ => _ is NewEvent).Count() > 1)
            {
                throw new MultipleNewException();
            }

            if (events.Value.First() is NewEvent newEvent)
            {
                return new Game(
                    newEvent.GameId,
                    newEvent.Version,
                    new NonEmptySet<PlayerRole>(new PlayerRole[] {
                        new PlayerRole(
                            newEvent.AdminId,
                            Role.Admin
                        )
                    }),
                    newEvent.Cards,
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