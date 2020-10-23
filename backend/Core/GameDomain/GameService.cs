using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Shared;

namespace Core.GameDomain
{
    public class NoNewException : Exception { }
    public class NoActiveRoundException : Exception { }
    public class PlayerConflictException : Exception { }
    public class ActiveRoundConflictException : Exception { }
    public class NoPlayerException : Exception { }
    public class NoCardException : Exception { }
    public class NotAllPlayersSelectedException : Exception { }
    public class NotNextVersionException : Exception { }
    public class NotSupportedEventException : Exception { }

    public class GameService : IGameService
    {
        private readonly IEventRepository eventRepository;

        public GameService(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<Game> AddPlayer(AddPlayerEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> EndRound(EndRoundEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> New(NewEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> SelectCard(SelectCardEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> NewRound(NewRoundEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        private Game FromEvent(IList<Event> events)
        {
            if (!(events.First() is NewEvent newEvent))
            {
                throw new NoNewException();
            }

            return events.Skip(1).Aggregate(new Game(
                newEvent.GameId,
                newEvent.Version,
                new PlayerRoleSet(
                    new PlayerRole(
                        newEvent.AdminId,
                        Role.Admin
                    )
                ),
                newEvent.Cards,
                null,
                new Set<CompletedRound>(Array.Empty<CompletedRound>())
            ), (acc, @event) =>
            {
                if (!acc.IsNextVersion(@event.Version))
                {
                    throw new NotNextVersionException();
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
                           addPlayerEvent.Version,
                           acc.PlayerRoles.AddPlayer(addPlayerEvent.PlayerId),
                           acc.Cards,
                           acc.ActiveRound,
                           acc.CompletedRounds
                       );
                    case NewRoundEvent newRoundEvent:
                        if (acc.IsActiveRound())
                        {
                            throw new ActiveRoundConflictException();
                        }

                        return new Game(
                           acc.Id,
                           newRoundEvent.Version,
                           acc.PlayerRoles,
                           acc.Cards,
                           new Round(
                               newRoundEvent.RoundId,
                               newRoundEvent.RoundName,
                               new PlayerCardSet()
                           ),
                           acc.CompletedRounds
                        );
                    case SelectCardEvent selectCardEvent:
                        if (!acc.IsActiveRound())
                        {
                            throw new NoActiveRoundException();
                        }

                        if (!acc.HasCard(selectCardEvent.PlayerCard.CardId))
                        {
                            throw new NoCardException();
                        }

                        if (!acc.HasPlayer(selectCardEvent.PlayerCard.PlayerId))
                        {
                            throw new NoPlayerException();
                        }

                        return new Game(
                            acc.Id,
                            selectCardEvent.Version,
                            acc.PlayerRoles,
                            acc.Cards,
                            new Round(
                                acc.ActiveRound.Value.Id,
                                acc.ActiveRound.Value.Name,
                                acc.ActiveRound.Value.PlayerCards.AddPlayerCard(selectCardEvent.PlayerCard)
                            ),
                            acc.CompletedRounds
                        );
                    case EndRoundEvent endRoundEvent:
                        if (!acc.IsActiveRound())
                        {
                            throw new NoActiveRoundException();
                        }

                        if (!acc.HasCard(endRoundEvent.ResultCardId))
                        {
                            throw new NoCardException();
                        }

                        if (!acc.HasAllPlayersSelected())
                        {
                            throw new NotAllPlayersSelectedException();
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
                                    acc.ActiveRound.Value.PlayerCards,
                                    endRoundEvent.ResultCardId
                                )
                            )
                        );
                    default:
                        throw new NotSupportedEventException();
                }
            });
        }
    }
}
