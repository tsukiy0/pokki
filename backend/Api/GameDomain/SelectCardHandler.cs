using System;
using System.Threading.Tasks;
using Core.GameDomain;
using Core.UserDomain;

namespace Api.GameDomain
{
    public struct SelectCardRequest
    {
        public struct PlayerCardInternal
        {
            public string PlayerId { get; set; }
            public string CardId { get; set; }
        }

        public string GameId { get; set; }
        public PlayerCardInternal PlayerCard { get; set; }
    }

    public class SelectCardHandler : BaseHandler<SelectCardRequest, GameResponse>
    {
        private readonly IGameService gameService;

        public SelectCardHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        protected override async Task<GameResponse> Handle(SelectCardRequest request)
        {
            var game = await gameService.SelectCard(new SelectCardEvent(
                new GameId(Guid.Parse(request.GameId)),
                new PlayerCard(
                    new UserId(Guid.Parse(request.PlayerCard.PlayerId)),
                    new CardId(Guid.Parse(request.PlayerCard.CardId))
                )
            ));

            return GameResponse.From(game);
        }
    }
}
