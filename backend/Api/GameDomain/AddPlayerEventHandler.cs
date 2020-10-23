using System;
using System.Threading.Tasks;
using Core.GameDomain;
using Core.UserDomain;

namespace Api.GameDomain
{
    public struct AddPlayerEventRequest
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }
    }

    public class AddPlayerEventHandler : BaseHandler<AddPlayerEventRequest, GameResponse>
    {
        private readonly IGameService gameService;

        public AddPlayerEventHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        protected override async Task<GameResponse> Handle(AddPlayerEventRequest request)
        {
            var game = await gameService.AddPlayer(new AddPlayerEvent(
                new GameId(Guid.Parse(request.GameId)),
                new UserId(Guid.Parse(request.PlayerId))
            ));

            return GameResponse.From(game);
        }
    }

}
