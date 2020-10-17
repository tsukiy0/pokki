using System;
using System.Linq;
using Core.Shared;

namespace Core.Game
{
    public class NoNewGameException : Exception { }
    public class MultipleNewException : Exception { }
    public class NotAscendingEventOrderException : Exception { }
    public class NoActiveRoundException : Exception { }
    public class NotSupportedEventException : Exception { }
    public class PlayerConflictException : Exception { }
    public class ActiveRoundConflictException : Exception { }
    public class NoPlayerException : Exception { }
    public class NoCardException : Exception { }
    public class NotAllPlayersSelected : Exception { }

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
                                if (acc.HasPlayer(addPlayerEvent.PlayerId))
                                {
                                    throw new PlayerConflictException();
                                }

                                return new Game(
                                    acc.Id,
                                    @event.Version,
                                    acc.PlayerRoles.ConcatOne(
                                        new PlayerRole(
                                            addPlayerEvent.PlayerId,
                                            Role.Player
                                        )
                                    ),
                                    acc.Cards,
                                    acc.ActiveRound,
                                    acc.CompletedRounds
                                );
                            case NewRoundEvent newRoundEvent:
                                if (acc.HasActiveRound())
                                {
                                    throw new ActiveRoundConflictException();
                                }

                                return new Game(
                                    acc.Id,
                                    @event.Version,
                                    acc.PlayerRoles,
                                    acc.Cards,
                                    newRoundEvent.Round,
                                    acc.CompletedRounds
                                );
                            case SelectCardEvent selectCardEvent:
                                if (!acc.HasCard(selectCardEvent.PlayerCard.CardId))
                                {
                                    throw new NoCardException();
                                }

                                if (!acc.HasPlayer(selectCardEvent.PlayerCard.PlayerId))
                                {
                                    throw new NoPlayerException();
                                }

                                if (!acc.HasActiveRound())
                                {
                                    throw new NoActiveRoundException();
                                }

                                return new Game(
                                    acc.Id,
                                    @event.Version,
                                    acc.PlayerRoles,
                                    acc.Cards,
                                    new Round(
                                        acc.ActiveRound.Value.Id,
                                        acc.ActiveRound.Value.Name,
                                        acc.ActiveRound.Value.PlayerCards.ConcatOne(selectCardEvent.PlayerCard)
                                    ),
                                    acc.CompletedRounds
                                );
                            case EndRoundEvent endRoundEvent:
                                if (!acc.HasCard(endRoundEvent.ResultCardId))
                                {
                                    throw new NoCardException();
                                }

                                if (!acc.HasActiveRound())
                                {
                                    throw new NoActiveRoundException();
                                }

                                if (!acc.HasAllPlayersSelected())
                                {
                                    throw new NotAllPlayersSelected();
                                }

                                return new Game(
                                    acc.Id,
                                    @event.Version,
                                    acc.PlayerRoles,
                                    acc.Cards,
                                    null,
                                    acc.CompletedRounds.ConcatOne(
                                        new CompletedRound(
                                            acc.ActiveRound.Value.Id,
                                            acc.ActiveRound.Value.Name,
                                            new NonEmptySet<PlayerCard>(acc.ActiveRound.Value.PlayerCards.Value),
                                            endRoundEvent.ResultCardId
                                        )
                                    )
                                );
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