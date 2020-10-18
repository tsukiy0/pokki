using System.Threading.Tasks;
using Core.Game;

namespace Infrastructure.EventRepository
{
    public interface IEventRepository
    {
        Task AppendNewEvent(NewEvent @event);
        Task AppendAddPlayerEvent(AddPlayerEvent @event);
        Task AppendSelectCardEvent(SelectCardEvent @event);
        Task AppendEndRoundEvent(EndRoundEvent @event);
        Task<EventList> GetEvents(GameId gameId);
    }
}
