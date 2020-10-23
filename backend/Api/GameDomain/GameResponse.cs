using System.Collections.Generic;
using System.Linq;
using Core.GameDomain;

namespace Api.GameDomain
{
    public struct GameResponse
    {
        public struct PlayerRole
        {
            public string PlayerId { get; set; }
            public Role Role { get; set; }
        }

        public struct Card
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public struct PlayerCard
        {
            public string PlayerId { get; set; }
            public string CardId { get; set; }
        }

        public struct Round
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public IList<PlayerCard> PlayerCards { get; set; }
        }

        public struct CompletedRound
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public IList<PlayerCard> PlayerCards { get; set; }
            public string ResultCardId { get; set; }
        }

        public string Id { get; set; }
        public GameStatus Status { get; set; }
        public IList<PlayerRole> PlayerRoles { get; set; }
        public IList<Card> Cards { get; set; }
        public IList<CompletedRound> CompletedRounds { get; set; }
        public Round? ActiveRound { get; set; }

        public static GameResponse From(Game game)
        {
            return new GameResponse
            {
                Id = game.Id.Value.ToString(),
                Status = game.Status,
                PlayerRoles = game.PlayerRoles.Value.Select(playerRole => new PlayerRole
                {
                    PlayerId = playerRole.PlayerId.Value.ToString(),
                    Role = playerRole.Role
                }).ToArray(),
                Cards = game.Cards.Value.Select(card => new Card
                {
                    Id = card.Id.Value.ToString(),
                    Name = card.Name
                }).ToArray(),
                CompletedRounds = game.CompletedRounds.Value.Select(completedRound => new CompletedRound
                {
                    Id = completedRound.Id.Value.ToString(),
                    Name = completedRound.Name,
                    PlayerCards = completedRound.PlayerCards.Value.Select(playerCard => new PlayerCard
                    {
                        PlayerId = playerCard.PlayerId.Value.ToString(),
                        CardId = playerCard.CardId.Value.ToString()
                    }).ToArray(),
                    ResultCardId = completedRound.ResultCardId.Value.ToString()
                }).ToArray(),
                ActiveRound = game.ActiveRound.HasValue ? (Round?)new Round
                {
                    Id = game.ActiveRound.Value.Id.Value.ToString(),
                    Name = game.ActiveRound.Value.Name,
                    PlayerCards = game.ActiveRound.Value.PlayerCards.Value.Select(playerCard => new PlayerCard
                    {
                        PlayerId = playerCard.PlayerId.Value.ToString(),
                        CardId = playerCard.CardId.Value.ToString()
                    }).ToArray()
                } : null
            };
        }
    }
}
