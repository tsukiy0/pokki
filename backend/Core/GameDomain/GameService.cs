using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.GameDomain
{
    public class NoNewException : Exception { }
    public class NoActiveRoundException : Exception { }
    public class PlayerConflictException : Exception { }
    public class ActiveRoundConflictException : Exception { }
    public class NoPlayerException : Exception { }
    public class NoCardException : Exception { }
    public class NotAllPlayersSelectedException : Exception { }
    public class NotSupportedEventException : Exception { }
    public class NotPendingException : Exception { }

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
                GameStatus.PENDING,
                new PlayerRoleSet(
                    new PlayerRole(
                        newEvent.AdminId,
                        Role.Admin
                    )
                ),
                newEvent.Cards,
                null,
                new CompletedRoundSet()
            ), (acc, @event) =>
            {
                switch (@event)
                {
                    case AddPlayerEvent addPlayerEvent:
                        if (acc.Status != GameStatus.PENDING)
                        {
                            throw new NotPendingException();
                        }

                        if (acc.PlayerRoles.HasPlayer(addPlayerEvent.PlayerId))
                        {
                            throw new PlayerConflictException();
                        }

                        return new Game(
                           acc.Id,
                           GameStatus.PENDING,
                           acc.PlayerRoles.AddPlayer(addPlayerEvent.PlayerId),
                           acc.Cards,
                           acc.ActiveRound,
                           acc.CompletedRounds
                       );
                    case NewRoundEvent newRoundEvent:
                        if (acc.Status == GameStatus.ACTIVE)
                        {
                            throw new ActiveRoundConflictException();
                        }

                        return new Game(
                           acc.Id,
                           GameStatus.ACTIVE,
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
                        if (acc.Status != GameStatus.ACTIVE)
                        {
                            throw new NoActiveRoundException();
                        }

                        if (!acc.Cards.HasCard(selectCardEvent.PlayerCard.CardId))
                        {
                            throw new NoCardException();
                        }

                        if (!acc.PlayerRoles.HasPlayer(selectCardEvent.PlayerCard.PlayerId))
                        {
                            throw new NoPlayerException();
                        }

                        return new Game(
                            acc.Id,
                            GameStatus.ACTIVE,
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
                        if (acc.Status != GameStatus.ACTIVE)
                        {
                            throw new NoActiveRoundException();
                        }

                        if (!acc.Cards.HasCard(endRoundEvent.ResultCardId))
                        {
                            throw new NoCardException();
                        }

                        if (!acc.HasAllPlayersSelected())
                        {
                            throw new NotAllPlayersSelectedException();
                        }

                        return new Game(
                            acc.Id,
                            GameStatus.INACTIVE,
                            acc.PlayerRoles,
                            acc.Cards,
                            null,
                            acc.CompletedRounds.AddRound(
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
