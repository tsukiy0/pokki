using System;
using System.Threading.Tasks;
using Core.GameDomain;
using Core.UserDomain;

namespace Api.GameDomain
{
    public struct AddPlayerRequest
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
    }

    public class AddPlayerHandler : BaseHandler<AddPlayerRequest, GameResponse>
    {
        private readonly IGameService gameService;

        public AddPlayerHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        protected override async Task<GameResponse> Handle(AddPlayerRequest request)
        {
            var game = await gameService.AddPlayer(new AddPlayerEvent(
                new GameId(Guid.Parse(request.GameId)),
                new UserId(Guid.Parse(request.PlayerId))
            ));

            return GameResponse.From(game);
        }
    }

}
