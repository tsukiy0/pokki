using System.Threading.Tasks;

namespace Core.Game
{
    public interface IGameService
    {
        Task<Game> New(NewEvent @event);
        Task<Game> AddPlayer(AddPlayerEvent @event);
        Task<Game> NewRound(NewRoundEvent @event);
        Task<Game> EndRound(EndRoundEvent @event);
    }
}
