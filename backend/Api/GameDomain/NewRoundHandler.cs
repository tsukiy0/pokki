using System;
using System.Threading.Tasks;
using Core.GameDomain;

namespace Api.GameDomain
{
    public struct NewRoundRequest
    {
        public string GameId { get; set; }
        public string RoundId { get; set; }
        public string RoundName { get; set; }
    }

    public class NewRoundHandler : BaseHandler<NewRoundRequest, GameResponse>
    {
        private readonly IGameService gameService;

        public NewRoundHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        protected override async Task<GameResponse> Handle(NewRoundRequest request)
        {
            var game = await gameService.NewRound(new NewRoundEvent(
                new GameId(Guid.Parse(request.GameId)),
                new RoundId(Guid.Parse(request.RoundId)),
                request.RoundName
            ));

            return GameResponse.From(game);
        }
    }

}
