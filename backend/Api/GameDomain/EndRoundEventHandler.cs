using System;
using System.Threading.Tasks;
using Core.GameDomain;

namespace Api.GameDomain
{
    public struct EndRoundEventRequest
    {
        public string GameId { get; set; }
        public string ResultCardId { get; set; }
    }

    public class EndRoundEventHandler : BaseHandler<EndRoundEventRequest, GameResponse>
    {
        private readonly IGameService gameService;

        public EndRoundEventHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        protected override async Task<GameResponse> Handle(EndRoundEventRequest request)
        {
            var game = await gameService.EndRound(new EndRoundEvent(
                new GameId(Guid.Parse(request.GameId)),
                new CardId(Guid.Parse(request.ResultCardId))
            ));

            return GameResponse.From(game);
        }
    }

}
