using System;
using System.Linq;
using Core.Shared;

namespace Core.Game
{
    public class NoNewGameException : Exception { }
    public class MultipleNewException : Exception { }
    public class NotAscendingEventOrderException : Exception { }
    public class NoActiveRoundException : Exception { }

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
                                return new Game(
                                    acc.Id,
                                    @event.Version,
                                    new NonEmptySet<PlayerRole>(
                                        acc.PlayerRoles.Value.Concat(new PlayerRole[]{
                                        new PlayerRole(
                                            addPlayerEvent.PlayerId,
                                            Role.Player
                                        )
                                        }).ToList()
                                    ),
                                    acc.Cards,
                                    acc.ActiveRound,
                                    acc.CompletedRounds
                                );
                            case NewRoundEvent newRoundEvent:
                                // @TODO check round already started
                                return new Game(
                                    acc.Id,
                                    @event.Version,
                                    acc.PlayerRoles,
                                    acc.Cards,
                                    newRoundEvent.Round,
                                    acc.CompletedRounds
                                );
                            case SelectCardEvent selectCardEvent when !acc.ActiveRound.HasValue:
                                throw new NoActiveRoundException();
                            case SelectCardEvent selectCardEvent:
                                return new Game(
                                    acc.Id,
                                    @event.Version,
                                    acc.PlayerRoles,
                                    acc.Cards,
                                    new Round(
                                        acc.ActiveRound.Value.Id,
                                        acc.ActiveRound.Value.Name,
                                        new Set<PlayerCard>(
                                            acc.ActiveRound.Value.PlayerCards.Value.Concat(new PlayerCard[] {
                                            selectCardEvent.PlayerCard
                                            }).ToList()
                                        )
                                    ),
                                    acc.CompletedRounds
                                );
                        }

                        return new Game(
                            acc.Id,
                            @event.Version,
                            acc.PlayerRoles,
                            acc.Cards,
                            acc.ActiveRound,
                            acc.CompletedRounds
                        );
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