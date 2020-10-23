using System;
using System.Threading.Tasks;
using Core.GameDomain;
using Core.UserDomain;

namespace Api.GameDomain
{
    public struct SelectCardEventRequest
    {
        public struct PlayerCardInternal
        {
            public string PlayerId { get; set; }
            public string CardId { get; set; }
        }

        public string GameId { get; set; }
        public PlayerCardInternal PlayerCard { get; set; }
    }

    public class SelectCardEventHandler : BaseHandler<SelectCardEventRequest, GameResponse>
    {
        private readonly IGameService gameService;

        public SelectCardEventHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        protected override async Task<GameResponse> Handle(SelectCardEventRequest request)
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
