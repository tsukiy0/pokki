using System.Threading.Tasks;
using Core.GameDomain;
using Infrastructure.GameDomain.EventRepository;

namespace Infrastructure.GameDomain
{
    public class GameService : IGameService
    {
        private readonly IEventRepository eventRepository;

        public GameService(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        private async Task<Game> GetGame(GameId gameId)
        {
            var events = await eventRepository.ListEvents(gameId);
            return Game.FromEvent(events);
        }

        public async Task<Game> AddPlayer(AddPlayerEvent @event)
        {
            var game = await GetGame(@event.GameId);

            var newGame = game.AddPlayer(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Game> EndRound(EndRoundEvent @event)
        {
            var game = await GetGame(@event.GameId);

            var newGame = game.EndRound(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Game> New(NewEvent @event)
        {
            var game = Game.New(@event);
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> SelectCard(SelectCardEvent @event)
        {
            var game = await GetGame(@event.GameId);

            var newGame = game.SelectCard(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Game> NewRound(NewRoundEvent @event)
        {
            var game = await GetGame(@event.GameId);

            var newGame = game.NewRound(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }
    }
}
