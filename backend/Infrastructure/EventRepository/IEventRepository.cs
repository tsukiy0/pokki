using Core.Game;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.EventRepository
{
    public interface IEventRepository
    {
        Task AppendNewEvent(NewEvent @event);
        Task AppendAddPlayerEvent(AddPlayerEvent @event);
        Task AppendSelectCardEvent(SelectCardEvent @event);
        Task AppendEndRoundEvent(EndRoundEvent @event);
        Task<IList<Event>> ListEvents(GameId gameId);
    }
}
