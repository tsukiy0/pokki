using System.Threading.Tasks;
using Core.Game;
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

        public async Task<Core.Game.Game> AddPlayer(AddPlayerEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = new EventList(events).Reduce();

            var newGame = game.AddPlayer(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Core.Game.Game> EndRound(EndRoundEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = new EventList(events).Reduce();

            var newGame = game.EndRound(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Core.Game.Game> New(NewEvent @event)
        {
            var game = Core.Game.Game.New(@event);
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Core.Game.Game> SelectCard(SelectCardEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = new EventList(events).Reduce();

            var newGame = game.SelectCard(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }

        public async Task<Core.Game.Game> NewRound(NewRoundEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = new EventList(events).Reduce();

            var newGame = game.NewRound(@event);
            await eventRepository.AppendEvent(@event);

            return newGame;
        }
    }
}
