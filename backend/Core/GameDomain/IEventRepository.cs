using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.GameDomain
{
    public interface IEventRepository
    {
        Task AppendEvent(Event @event);
        Task<IList<Event>> ListEvents(GameId gameId);
    }
}
