using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.GameDomain;
using Core.Shared;
using Core.UserDomain;
using Moq;
using Xunit;

namespace GameTests
{

    [Trait("Category", "Unit")]
    public class GameServiceTest
    {
        [Fact]
        public async Task New()
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
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(newEvent.GameId)).ReturnsAsync(Array.Empty<Event>());
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.New(newEvent);

            Assert.Equal(newEvent.GameId, actual.Id);
            Assert.Equal(newEvent.Version, actual.Version);
            Assert.Equal(newEvent.AdminId, actual.GetAdminId());
            Assert.Equal(newEvent.Cards, actual.Cards);
            Assert.Null(actual.ActiveRound);
            Assert.Equal(new Set<CompletedRound>(Array.Empty<CompletedRound>()), actual.CompletedRounds);
        }

        [Fact]
        public async Task AddPlayer()
        {
            var (events, game) = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                events[0].GameId,
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.AddPlayer(addPlayerEvent);

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
        public async Task AddPlayer_ThrowWhenNoNew()
        {
            var addPlayerEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(addPlayerEvent.GameId)).ReturnsAsync(Array.Empty<Event>());
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoNewException>(() => service.AddPlayer(addPlayerEvent));
        }

        [Fact]
        public async Task AddPlayer_ThrowWhenPlayerExists()
        {
            var (events, game) = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                game.Id,
                new EventVersion(2),
                game.GetAdminId()
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<PlayerConflictException>(() => service.AddPlayer(addPlayerEvent));
        }

        [Fact]
        public async Task AddPlayer_ThrowWhenNotNextVersion()
        {
            var (events, game) = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                game.Id,
                new EventVersion(1),
                game.GetAdminId()
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotNextVersionException>(() => service.AddPlayer(addPlayerEvent));
        }

        [Fact]
        public async Task NewRound()
        {
            var (events, game) = GetNewGameWithPlayers();
            var newRoundEvent = new NewRoundEvent(
                game.Id,
                new EventVersion(3),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.NewRound(newRoundEvent);

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
        public async Task NewRoundEvent_ThrowWhenNoNew()
        {
            var newRoundEvent = new NewRoundEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(3),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(newRoundEvent.GameId)).ReturnsAsync(Array.Empty<Event>());
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoNewException>(() => service.NewRound(newRoundEvent));
        }

        [Fact]
        public async Task NewRound_ThrowWhenHasActiveRound()
        {
            var (events, game) = GetNewGameWithActiveRound();
            var newRoundEvent = new NewRoundEvent(
                game.Id,
                new EventVersion(4),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<ActiveRoundConflictException>(() => service.NewRound(newRoundEvent));
        }

        [Fact]
        public async Task NewRound_ThrowWhenNotNextVersion()
        {
            var (events, game) = GetNewGameWithPlayers();
            var newRoundEvent = new NewRoundEvent(
                game.Id,
                new EventVersion(2),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotNextVersionException>(() => service.NewRound(newRoundEvent));
        }

        [Fact]
        public async Task SelectCard()
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
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.SelectCard(selectCardEvent);

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
        public async Task SelectCard_ThrowWhenNotNew()
        {
            var selectCardEvent = new SelectCardEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(4),
                new PlayerCard(
                    new UserId(Guid.NewGuid()),
                    new CardId(Guid.NewGuid())
                )
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(selectCardEvent.GameId)).ReturnsAsync(Array.Empty<Event>());
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoNewException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenNoActiveRound()
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
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoActiveRoundException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenNoPlayer()
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
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoPlayerException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenNoCard()
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
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoCardException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenNotNextVersion()
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
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotNextVersionException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenPlayerAlreadySelectedCard()
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
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<PlayerCardConflictException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task EndRound()
        {
            var (events, game) = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(6),
                game.Cards.Value[0].Id
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.EndRound(endRoundEvent);

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
        public async Task EndRoundEvent_ThrowWhenNotNew()
        {
            var endRoundEvent = new EndRoundEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(6),
                new CardId(Guid.NewGuid())
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(endRoundEvent.GameId)).ReturnsAsync(Array.Empty<Event>());
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoNewException>(() => service.EndRound(endRoundEvent));
        }

        [Fact]
        public async Task EndRound_ThrowWhenNoCard()
        {
            var (events, game) = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(6),
                new CardId(Guid.NewGuid())
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoCardException>(() => service.EndRound(endRoundEvent));
        }

        [Fact]
        public async Task EndRound_ThrowWhenNotNextVersion()
        {
            var (events, game) = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(5),
                new CardId(Guid.NewGuid())
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotNextVersionException>(() => service.EndRound(endRoundEvent));
        }

        [Fact]
        public async Task EndRound_ThrowWhenNoActiveRound()
        {
            var (events, game) = GetNewGameWithPlayers();
            var endRoundEvent = new EndRoundEvent(
                game.Id,
                new EventVersion(3),
                game.Cards.Value[0].Id
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoActiveRoundException>(() => service.EndRound(endRoundEvent));
        }

        [Fact]
        public async Task EndRound_ThrowWhenNotAllPlayersSelected()
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
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(game.Id)).ReturnsAsync(events.ConcatOne(selectCardEvent));
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotAllPlayersSelectedException>(() => service.EndRound(endRoundEvent));
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
