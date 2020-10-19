using Core.GameDomain;
using Core.Shared;
using Core.UserDomain;
using System;
using System.Collections.Generic;
using Xunit;
using Core;

namespace CoreTests
{
    [Trait("Category", "Unit")]
    public class GameTest
    {
        [Fact]
        public void ThrowWhenNoNew()
        {
            var newEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );

            Assert.Throws<NoNewException>(() => Game.FromEvent(new Event[] { newEvent }));
        }


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

            var actual = Game.FromEvent(new Event[] { newEvent });

            Assert.Equal(newEvent.GameId, actual.Id);
            Assert.Equal(newEvent.Version, actual.Version);
            Assert.Equal(newEvent.AdminId, actual.GetAdminId());
            Assert.Equal(newEvent.Cards, actual.Cards);
            Assert.Null(actual.ActiveRound);
            Assert.Equal(new Set<CompletedRound>(Array.Empty<CompletedRound>()), actual.CompletedRounds);
        }

        [Fact]
        public void AddPlayer()
        {
            var (events, game) = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                events[0].GameId,
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );

            var actual = Game.FromEvent(events.ConcatOne(addPlayerEvent));

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
        public void AddPlayer_ThrowWhenPlayerExists()
        {
            var (events, game) = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                game.Id,
                new EventVersion(2),
                game.GetAdminId()
            );

            Assert.Throws<PlayerConflictException>(() => Game.FromEvent(events.ConcatOne(addPlayerEvent)));
        }

        [Fact]
        public void AddPlayer_ThrowWhenNotNextVersion()
        {
            var (events, game) = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                game.Id,
                new EventVersion(1),
                game.GetAdminId()
            );

            Assert.Throws<NotNextVersionException>(() => Game.FromEvent(events.ConcatOne(addPlayerEvent)));
        }

        [Fact]
        public void NewRound()
        {
            var (events, game) = GetNewGameWithPlayers();
            var newRoundEvent = new NewRoundEvent(
                game.Id,
                new EventVersion(3),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );

            var actual = Game.FromEvent(events.ConcatOne(newRoundEvent));

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
            var (events, game) = GetNewGameWithActiveRound();
            var newRoundEvent = new NewRoundEvent(
                game.Id,
                new EventVersion(4),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );

            Assert.Throws<ActiveRoundConflictException>(() => Game.FromEvent(events.ConcatOne(newRoundEvent)));
        }

        [Fact]
        public void NewRound_ThrowWhenNotNextVersion()
        {
            var (events, game) = GetNewGameWithPlayers();
            var newRoundEvent = new NewRoundEvent(
                game.Id,
                new EventVersion(2),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );

            Assert.Throws<NotNextVersionException>(() => Game.FromEvent(events.ConcatOne(newRoundEvent)));
        }

        [Fact]
        public void SelectCard()
        {
            var (events, game) = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(4),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            );

            var actual = Game.FromEvent(events.ConcatOne(selectCardEvent));

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
            var (events, game) = GetNewGame();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(2),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            );

            Assert.Throws<NoActiveRoundException>(() => Game.FromEvent(events.ConcatOne(selectCardEvent)));
        }

        [Fact]
        public void SelectCard_ThrowWhenNoPlayer()
        {
            var (events, game) = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(4),
                new PlayerCard(
                    new UserId(Guid.NewGuid()),
                    game.Cards.Value[0].Id
                )
            );

            Assert.Throws<NoPlayerException>(() => Game.FromEvent(events.ConcatOne(selectCardEvent)));
        }

        [Fact]
        public void SelectCard_ThrowWhenNoCard()
        {
            var (events, game) = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(4),
                new PlayerCard(
                    game.GetAdminId(),
                    new CardId(Guid.NewGuid())
                )
            );

            Assert.Throws<NoCardException>(() => Game.FromEvent(events.ConcatOne(selectCardEvent)));
        }

        [Fact]
        public void SelectCard_ThrowWhenNotNextVersion()
        {
            var (events, game) = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(3),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            );

            Assert.Throws<NotNextVersionException>(() => Game.FromEvent(events.ConcatOne(selectCardEvent)));
        }

        [Fact]
        public void SelectCard_ThrowWhenPlayerAlreadySelectedCard()
        {
            var (events, game) = GetNewGameWithSelectedCards();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(6),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            );

            Assert.Throws<PlayerCardConflictException>(() => Game.FromEvent(events.ConcatOne(selectCardEvent)));
        }

        [Fact]
        public void EndRound()
        {
            var (events, game) = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(6),
                game.Cards.Value[0].Id
            );

            var actual = Game.FromEvent(events.ConcatOne(endRoundEvent));

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
            var (events, game) = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(6),
                new CardId(Guid.NewGuid())
            );

            Assert.Throws<NoCardException>(() => Game.FromEvent(events.ConcatOne(endRoundEvent)));
        }

        [Fact]
        public void EndRound_ThrowWhenNotNextVersion()
        {
            var (events, game) = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(5),
                new CardId(Guid.NewGuid())
            );

            Assert.Throws<NotNextVersionException>(() => Game.FromEvent(events.ConcatOne(endRoundEvent)));
        }

        [Fact]
        public void EndRound_ThrowWhenNoActiveRound()
        {
            var (events, game) = GetNewGameWithPlayers();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(3),
                game.Cards.Value[0].Id
            );

            Assert.Throws<NoActiveRoundException>(() => Game.FromEvent(events.ConcatOne(endRoundEvent)));
        }

        [Fact]
        public void EndRound_ThrowWhenNotAllPlayersSelected()
        {
            var (events, game) = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                game.Id,
                new EventVersion(4),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            );
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(5),
                game.Cards.Value[0].Id
            );

            Assert.Throws<NotAllPlayersSelectedException>(() => Game.FromEvent(events.ConcatOne(selectCardEvent).ConcatOne(endRoundEvent)));
        }

        private (IList<Event>, Game) GetNewGame()
        {
            var events = new Event[] {
                new NewEvent(
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
                )
            };

            return (events, Game.FromEvent(events));
        }

        private (IList<Event>, Game) GetNewGameWithPlayers()
        {
            var (events, game) = GetNewGame();
            var newEvents = events.ConcatOne(new AddPlayerEvent(
                game.Id,
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            ));

            return (newEvents, Game.FromEvent(newEvents));
        }

        private (IList<Event>, Game) GetNewGameWithActiveRound()
        {
            var (events, game) = GetNewGameWithPlayers();
            var newEvents = events.ConcatOne(new NewRoundEvent(
                game.Id,
                new EventVersion(3),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            ));

            return (newEvents, Game.FromEvent(newEvents));
        }

        private (IList<Event>, Game) GetNewGameWithSelectedCards()
        {
            var (events, game) = GetNewGameWithActiveRound();
            var newEvents = events.ConcatOne(new SelectCardEvent(
                game.Id,
                new EventVersion(4),
                new PlayerCard(
                    game.GetAdminId(),
                    game.Cards.Value[0].Id
                )
            )).ConcatOne(new SelectCardEvent(
                events[0].GameId,
                new EventVersion(5),
                new PlayerCard(
                    game.PlayerRoles.Value[1].PlayerId,
                    game.Cards.Value[0].Id
                )
            ));

            return (newEvents, Game.FromEvent(newEvents));
        }
    }
}
