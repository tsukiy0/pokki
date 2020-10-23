using System;
using System.Threading.Tasks;
using Core.GameDomain;

namespace Api.GameDomain
{
    public struct EndRoundRequest
    {
        public string GameId { get; set; }
        public string ResultCardId { get; set; }
    }

    public class EndRoundHandler : BaseHandler<EndRoundRequest, GameResponse>
    {
        private readonly IGameService gameService;

        public EndRoundHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        protected override async Task<GameResponse> Handle(EndRoundRequest request)
        {
            var game = await gameService.EndRound(new EndRoundEvent(
                new GameId(Guid.Parse(request.GameId)),
                new CardId(Guid.Parse(request.ResultCardId))
            ));

            return GameResponse.From(game);
        }
    }

}
