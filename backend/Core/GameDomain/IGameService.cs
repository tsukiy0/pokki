using System.Threading.Tasks;

namespace Core.GameDomain
{
    public interface IGameService
    {
        Task<Game> New(NewEvent @event);
        Task<Game> AddPlayer(AddPlayerEvent @event);
        Task<Game> NewRound(NewRoundEvent @event);
        Task<Game> SelectCard(SelectCardEvent @event);
        Task<Game> EndRound(EndRoundEvent @event);
    }
}
