using System.Threading.Tasks;

namespace Core.GameDomain
{
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
            var game = Game.FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> EndRound(EndRoundEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = Game.FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> New(NewEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = Game.FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> SelectCard(SelectCardEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = Game.FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }

        public async Task<Game> NewRound(NewRoundEvent @event)
        {
            var events = await eventRepository.ListEvents(@event.GameId);
            var game = Game.FromEvent(events.ConcatOne(@event));
            await eventRepository.AppendEvent(@event);
            return game;
        }
    }
}
