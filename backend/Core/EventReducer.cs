using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class NoNewGameEventException : Exception { }

    public class EventReducer
    {
        public Game Reduce(NonEmptySet<Event> events)
        {
            var firstEvent = events.Value.First();

            if (firstEvent is NewGameEvent newGameEvent)
            {
                return new Game(newGameEvent.GameId, newGameEvent.Admin.Id, new NonEmptySet<Person>(new List<Person> { newGameEvent.Admin }), null, null, new Set<Round>(new List<Round> { }));
            }
            else
            {
                throw new NoNewGameEventException();
            }
        }
    }
}
