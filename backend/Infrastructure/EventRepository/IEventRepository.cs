using Core.Game;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.EventRepository
{
    public interface IEventRepository
    {
        Task AppendEvent(Event @event);
        Task<IList<Event>> ListEvents(GameId gameId);
    }
}
