using System.Threading.Tasks;
using Core.Game;
using Core.Game.Models;
using Infrastructure.Game.EventRepository;

namespace Infrastructure.Game
{
    public class GameService : IGameService
    {
        private readonly IEventRepository eventRepository;

        public GameService(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        private async Task<Core.Game.Models.Game> GetGame(GameId gameId)
        {
            var events = await eventRepository.ListEvents(gameId);
            return Core.Game.Models.Game.FromEvent(events);
        }

        public async Task<Core.Game.Models.Game> AddPlayer(AddPlayerEvent @event)
        {
            var game = await GetGame(@event.GameId);

            var newGame = game.AddPlayer(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Core.Game.Models.Game> EndRound(EndRoundEvent @event)
        {
            var game = await GetGame(@event.GameId);

            var newGame = game.EndRound(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Core.Game.Models.Game> New(NewEvent @event)
        {
            var game = Core.Game.Models.Game.New(@event);
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Core.Game.Models.Game> SelectCard(SelectCardEvent @event)
        {
            var game = await GetGame(@event.GameId);

            var newGame = game.SelectCard(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Core.Game.Models.Game> NewRound(NewRoundEvent @event)
        {
            var game = await GetGame(@event.GameId);

            var newGame = game.NewRound(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }
    }
}
