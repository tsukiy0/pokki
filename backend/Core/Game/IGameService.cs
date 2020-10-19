using System.Threading.Tasks;
using Core.Game.Models;

namespace Core.Game
{
    public interface IGameService
    {
        Task<Models.Game> New(NewEvent @event);
        Task<Models.Game> AddPlayer(AddPlayerEvent @event);
        Task<Models.Game> NewRound(NewRoundEvent @event);
        Task<Models.Game> SelectCard(SelectCardEvent @event);
        Task<Models.Game> EndRound(EndRoundEvent @event);
    }
}
