using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.GameDomain;
using Core.UserDomain;

namespace Api.GameDomain
{
    public struct NewEventRequest
    {
        public struct Card
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public string GameId { get; set; }
        public string AdminId { get; set; }
        public IList<Card> Cards { get; set; }
    }

    public class NewEventHandler : BaseHandler<NewEventRequest, GameResponse>
    {
        private readonly IGameService gameService;

        public NewEventHandler(IGameService gameService)
        {
            this.gameService = gameService;
        }

        protected override async Task<GameResponse> Handle(NewEventRequest request)
        {
            var game = await gameService.New(new NewEvent(
                new GameId(Guid.Parse(request.GameId)),
                new UserId(Guid.Parse(request.AdminId)),
                new CardSet(request.Cards.Select(card => new Card(
                    new CardId(Guid.Parse(card.Id)),
                    card.Name
                )).ToArray())
            ));

            return GameResponse.From(game);
        }
    }

}
