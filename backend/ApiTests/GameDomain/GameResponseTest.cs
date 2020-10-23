using System;
using System.Text.Json;
using Api.GameDomain;
using Core.GameDomain;
using Core.UserDomain;
using Xunit;

namespace ApiTests
{
    [Trait("Category", "Unit")]
    public class GameResponseTest
    {
        [Fact]
        public void From()
        {
            var gameId = new GameId(Guid.NewGuid());
            var playerId1 = new UserId(Guid.NewGuid());
            var playerId2 = new UserId(Guid.NewGuid());
            var cardId1 = new CardId(Guid.NewGuid());
            var cardId2 = new CardId(Guid.NewGuid());
            var roundId1 = new RoundId(Guid.NewGuid());
            var roundId2 = new RoundId(Guid.NewGuid());
            var game = new Game(
                gameId,
                GameStatus.ACTIVE,
                new PlayerRoleSet(
                    new PlayerRole(
                        playerId1,
                        Role.Admin
                    ),
                    new PlayerRole(
                        playerId2,
                        Role.Player
                    )
                ),
                new CardSet(
                    new Card(
                        cardId1,
                        "M"
                    ),
                    new Card(
                        cardId2,
                        "L"
                    )
                ),
                new Round(
                    roundId1,
                    "SM123",
                    new PlayerCardSet(
                        new PlayerCard(
                            playerId1,
                            cardId1
                        ),
                        new PlayerCard(
                            playerId2,
                            cardId2
                        )
                    )
                ),
                new CompletedRoundSet(
                    new CompletedRound(
                        roundId2,
                        "SM123",
                        new PlayerCardSet(
                            new PlayerCard(
                                playerId1,
                                cardId1
                            ),
                            new PlayerCard(
                                playerId2,
                                cardId2
                            )
                        ),
                        cardId2
                    )
                )
            );

            var actual = GameResponse.From(game);

            var expected = new GameResponse
            {
                Id = gameId.Value.ToString(),
                Status = GameStatus.ACTIVE,
                PlayerRoles = new[]{
                    new GameResponse.PlayerRole {
                        PlayerId = playerId1.Value.ToString(),
                        Role = Role.Admin
                    },
                    new GameResponse.PlayerRole {
                        PlayerId = playerId2.Value.ToString(),
                        Role = Role.Player
                    },
                },
                Cards = new[]{
                    new GameResponse.Card {
                        Id = cardId1.Value.ToString(),
                        Name = "M"
                    },
                    new GameResponse.Card {
                        Id = cardId2.Value.ToString(),
                        Name = "L"
                    }
                },
                ActiveRound = new GameResponse.Round
                {
                    Id = roundId1.Value.ToString(),
                    Name = "SM123",
                    PlayerCards = new[]{
                        new GameResponse.PlayerCard {
                            PlayerId = playerId1.Value.ToString(),
                            CardId = cardId1.Value.ToString()
                        },
                        new GameResponse.PlayerCard {
                            PlayerId = playerId2.Value.ToString(),
                            CardId = cardId2.Value.ToString()
                        }
                    }
                },
                CompletedRounds = new[]{
                    new GameResponse.CompletedRound {
                        Id = roundId2.Value.ToString(),
                        Name = "SM123",
                        PlayerCards = new[]{
                            new GameResponse.PlayerCard {
                                PlayerId = playerId1.Value.ToString(),
                                CardId = cardId1.Value.ToString()
                            },
                            new GameResponse.PlayerCard {
                                PlayerId = playerId2.Value.ToString(),
                                CardId = cardId2.Value.ToString()
                            }
                        },
                        ResultCardId = cardId2.Value.ToString()
                    }
                }
            };

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }
    }
}
