using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class GameNotStartedException : Exception { }
    public class CardsAlreadyExistException : Exception { }
    public class GameAlreadyStartedException : Exception { }
    public class RoundAlreadyStartedException : Exception { }

    public class EventReducer
    {
        public Game Reduce(NonEmptySet<Event> events)
        {
            var firstEvent = events.Value.First();

            if (firstEvent is NewGameEvent newGameEvent)
            {
                return events.Value.Skip(1).Aggregate<Event, Game>(
                    new Game(newGameEvent.GameId, newGameEvent.Admin.Id, new NonEmptySet<Person>(new List<Person> { newGameEvent.Admin }), null, null, new Set<Round>(new List<Round> { })),
                    (acc, item) =>
                    {
                        switch (item)
                        {
                            case AddCardsEvent e when acc.Cards == null:
                                throw new CardsAlreadyExistException();
                            case NewRoundEvent e when acc.ActiveRound != null:
                                throw new RoundAlreadyStartedException();
                            case AddPersonEvent e:
                                return new Game(
                                    acc.Id,
                                    acc.AdminId,
                                    new NonEmptySet<Person>(
                                        acc.People.Value.Concat(new List<Person> { e.Person }).ToList()
                                    ),
                                    acc.Cards,
                                    acc.ActiveRound,
                                    acc.FinishedRounds
                                );
                            case AddCardsEvent e:
                                return new Game(
                                    acc.Id,
                                    acc.AdminId,
                                    acc.People,
                                    e.Cards,
                                    acc.ActiveRound,
                                    acc.FinishedRounds
                                );
                            case NewRoundEvent e:
                                return new Game(
                                    acc.Id,
                                    acc.AdminId,
                                    acc.People,
                                    acc.Cards,
                                    e.Round,
                                    acc.FinishedRounds
                                );
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                );
            }
            else
            {
                throw new GameNotStartedException();
            }
        }
    }
}
