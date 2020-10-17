using System;
using System.Linq;
using Core.Shared;

namespace Core.Game
{
    public class NoNewGameException : Exception { }
    public class MultipleNewException : Exception { }
    public class NotAscendingEventOrderException : Exception { }
    public class NotSupportedEventException : Exception { }

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
                return events.Value.Skip(1).Aggregate(
                    new Game(
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
                    ), (acc, @event) =>
                    {
                        if (@event.Version.Value <= acc.Version.Value)
                        {
                            throw new NotAscendingEventOrderException();
                        }

                        switch (@event)
                        {
                            case AddPlayerEvent addPlayerEvent:
                                return acc.AddNewPlayer(addPlayerEvent);
                            case NewRoundEvent newRoundEvent:
                                return acc.NewRound(newRoundEvent);
                            case SelectCardEvent selectCardEvent:
                                return acc.SelectCard(selectCardEvent);
                            case EndRoundEvent endRoundEvent:
                                return acc.EndRound(endRoundEvent);
                            default:
                                throw new NotSupportedEventException();
                        }
                    }
                );
            }
            else
            {
                throw new NoNewGameException();
            }
        }
    }
}