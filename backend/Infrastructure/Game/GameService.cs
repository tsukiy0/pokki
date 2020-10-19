using System.Threading.Tasks;
using Core.Game;

namespace Infrastructure.Game
{
    public class GameService : IGameService
    {
        public Task<Core.Game.Game> AddPlayer(AddPlayerEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public Task<Core.Game.Game> EndRound(EndRoundEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public Task<Core.Game.Game> New(NewEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public Task<Core.Game.Game> SelectCard(SelectCardEvent @event)
        {
            throw new System.NotImplementedException();
        }

        public Task<Core.Game.Game> NewRound(NewRoundEvent @event)
        {
            throw new System.NotImplementedException();
        }
    }
}
