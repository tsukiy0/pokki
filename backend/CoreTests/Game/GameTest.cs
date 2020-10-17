using System;
using Core.Game;
using Core.Shared;
using Core.User;
using Xunit;

namespace CoreTests
{
    public class GameTest
    {
        [Fact]
        public void New()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
                new NonEmptySet<Card>(new Card[] {
                new Card(
                    new CardId(Guid.NewGuid()),
                    "M"
                ),
                new Card(
                    new CardId(Guid.NewGuid()),
                    "L"
                )
            }));

            var actual = Game.New(newEvent);

            Assert.Equal(newEvent.GameId, actual.Id);
            Assert.Equal(newEvent.Version, actual.Version);
            Assert.Equal(newEvent.AdminId, actual.GetAdminId());
            Assert.Equal(newEvent.Cards, actual.Cards);
            Assert.Null(actual.ActiveRound);
            Assert.Equal(new Set<CompletedRound>(Array.Empty<CompletedRound>()), actual.CompletedRounds);
        }

        [Fact]
        public void AddNewPlayer()
        {
            var game = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                game.Id,
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );

            var actual = game.AddNewPlayer(addPlayerEvent);

            Assert.Equal(
                new NonEmptySet<PlayerRole>(new PlayerRole[]{
                    new PlayerRole(
                        game.GetAdminId(),
                        Role.Admin
                    ),
                    new PlayerRole(
                        addPlayerEvent.PlayerId,
                        Role.Player
                    )
                }),
                actual.PlayerRoles
            );
        }

        [Fact]
        public void AddNewPlayer_ThrowWhenPlayerExists()
        {
            var game = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                game.Id,
                new EventVersion(2),
                game.GetAdminId()
            );

            Assert.Throws<PlayerConflictException>(() => game.AddNewPlayer(addPlayerEvent));
        }

        [Fact]
        public void NewRound()
        {
            var game = GetNewGameWithPlayers();
            var newRoundEvent = new NewRoundEvent(
                game.Id,
                new EventVersion(3),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );

            var actual = game.NewRound(newRoundEvent);

            Assert.Equal(
                new Round(
                    newRoundEvent.RoundId,
                    newRoundEvent.RoundName,
                    new Set<PlayerCard>(Array.Empty<PlayerCard>())
                ),
                actual.ActiveRound
            );
        }

        [Fact]
        public void NewRound_ThrowWhenHasActiveRound()
        {
            var game = GetNewGameWithActiveRound();
            var newRoundEvent = new NewRoundEvent(
                game.Id,
                new EventVersion(4),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );

            Assert.Throws<ActiveRoundConflictException>(() => game.NewRound(newRoundEvent));
        }

        [Fact]
        public void SelectCard()
        {
            var game = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(3),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            );

            var actual = game.SelectCard(selectCardEvent);

            Assert.Equal(
                    new NonEmptySet<PlayerCard>(
                        new PlayerCard[]{
                            selectCardEvent.PlayerCard
                        }
                    ),
                    actual.ActiveRound.Value.PlayerCards
                );
        }

        [Fact]
        public void SelectCard_ThrowWhenNoActiveRound()
        {
            var game = GetNewGame();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(3),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            );

            Assert.Throws<NoActiveRoundException>(() => game.SelectCard(selectCardEvent));
        }

        [Fact]
        public void SelectCard_ThrowWhenNoPlayer()
        {
            var game = GetNewGame();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(3),
                new PlayerCard(
                    new UserId(Guid.NewGuid()),
                    game.Cards.Value[0].Id
                )
            );

            Assert.Throws<NoPlayerException>(() => game.SelectCard(selectCardEvent));
        }

        [Fact]
        public void SelectCard_ThrowWhenNoCard()
        {
            var game = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(4),
                new PlayerCard(
                    game.GetAdminId(),
                    new CardId(Guid.NewGuid())
                )
            );

            Assert.Throws<NoCardException>(() => game.SelectCard(selectCardEvent));
        }

        [Fact]
        public void SelectCard_ThrowWhenPlayerAlreadySelectedCard()
        {
            var game = GetNewGameWithSelectedCards();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(6),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            );

            Assert.Throws<PlayerCardConflictException>(() => game.SelectCard(selectCardEvent));
        }

        [Fact]
        public void EndRound()
        {
            var game = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(6),
                game.Cards.Value[0].Id
            );

            var actual = game.EndRound(endRoundEvent);

            Assert.Null(actual.ActiveRound);
            Assert.Equal(
                new Set<CompletedRound>(
                    new CompletedRound[]{
                        new CompletedRound(
                            game.ActiveRound.Value.Id,
                            game.ActiveRound.Value.Name,
                            new NonEmptySet<PlayerCard>(game.ActiveRound.Value.PlayerCards.Value),
                            endRoundEvent.ResultCardId
                        )
                    }
                ),
                actual.CompletedRounds
            );
        }

        [Fact]
        public void EndRound_ThrowWhenNoCard()
        {
            var game = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(6),
                new CardId(Guid.NewGuid())
            );

            Assert.Throws<NoCardException>(() => game.EndRound(endRoundEvent));
        }

        [Fact]
        public void EndRound_ThrowWhenNoActiveRound()
        {
            var game = GetNewGameWithPlayers();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(3),
                game.Cards.Value[0].Id
            );

            Assert.Throws<NoActiveRoundException>(() => game.EndRound(endRoundEvent));
        }

        [Fact]
        public void EndRound_ThrowWhenNotAllPlayersSelected()
        {
            var game = GetNewGameWithActiveRound();
            game.SelectCard(new SelectCardEvent(
                game.Id,
                new EventVersion(4),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            ));
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(5),
                game.Cards.Value[0].Id
            );

            Assert.Throws<NotAllPlayersSelectedException>(() => game.EndRound(endRoundEvent));
        }

        private Game GetNewGame()
        {
            return Game.New(new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
                new NonEmptySet<Card>(new Card[] {
                    new Card(
                        new CardId(Guid.NewGuid()),
                        "M"
                    ),
                    new Card(
                        new CardId(Guid.NewGuid()),
                        "L"
                    )
                })
            ));
        }

        private Game GetNewGameWithPlayers()
        {
            var game = GetNewGame();

            return game.AddNewPlayer(new AddPlayerEvent(
                game.Id,
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            ));
        }

        private Game GetNewGameWithActiveRound()
        {
            var game = GetNewGameWithPlayers();

            return game.NewRound(new NewRoundEvent(
                game.Id,
                new EventVersion(3),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            ));
        }

        private Game GetNewGameWithSelectedCards()
        {
            var game = GetNewGameWithActiveRound();

            return game.SelectCard(new SelectCardEvent(
                game.Id,
                new EventVersion(4),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            )).SelectCard(new SelectCardEvent(
                game.Id,
                new EventVersion(5),
                new PlayerCard(
                    game.PlayerRoles.Value[1].PlayerId,
                    game.Cards.Value[0].Id
                )
            ));
        }
    }
}