using Core.GameDomain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.GameDomain.EventRepository
{
    public interface IEventRepository
    {
        Task AppendEvent(Event @event);
        Task<IList<Event>> ListEvents(GameId gameId);
    }
}
