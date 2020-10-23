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
        private readonly GameId gameId = new GameId(Guid.NewGuid());
        private readonly UserId adminId = new UserId(Guid.NewGuid());
        private readonly UserId playerId = new UserId(Guid.NewGuid());
        private readonly CardSet cards = new CardSet(
            new Card(
                new CardId(Guid.NewGuid()),
                "M"
            ),
            new Card(
                new CardId(Guid.NewGuid()),
                "L"
            )
        );
        private readonly RoundId roundId = new RoundId(Guid.NewGuid());
        private readonly string roundName = "SM123";


        [Fact]
        public async Task New()
        {
            var newEvent = new NewEvent(
                gameId,
                new EventVersion(1),
                adminId,
                cards
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(Array.Empty<Event>());
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
            var events = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                events[0].GameId,
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.AddPlayer(addPlayerEvent);

            Assert.Equal(
                new PlayerRoleSet(
                    new PlayerRole(
                        adminId,
                        Role.Admin
                    ),
                    new PlayerRole(
                        addPlayerEvent.PlayerId,
                        Role.Player
                    )
                ),
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
            var events = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                gameId,
                new EventVersion(2),
                adminId
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<PlayerConflictException>(() => service.AddPlayer(addPlayerEvent));
        }

        [Fact]
        public async Task AddPlayer_ThrowWhenNotNextVersion()
        {
            var events = GetNewGame();
            var addPlayerEvent = new AddPlayerEvent(
                gameId,
                new EventVersion(1),
                adminId
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotNextVersionException>(() => service.AddPlayer(addPlayerEvent));
        }

        [Fact]
        public async Task NewRound()
        {
            var events = GetNewGameWithPlayers();
            var newRoundEvent = new NewRoundEvent(
                gameId,
                new EventVersion(3),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.NewRound(newRoundEvent);

            Assert.Equal(
                new Round(
                    newRoundEvent.RoundId,
                    newRoundEvent.RoundName,
                    new PlayerCardSet()
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
            var events = GetNewGameWithActiveRound();
            var newRoundEvent = new NewRoundEvent(
                gameId,
                new EventVersion(4),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<ActiveRoundConflictException>(() => service.NewRound(newRoundEvent));
        }

        [Fact]
        public async Task NewRound_ThrowWhenNotNextVersion()
        {
            var events = GetNewGameWithPlayers();
            var newRoundEvent = new NewRoundEvent(
                gameId,
                new EventVersion(2),
                new RoundId(Guid.NewGuid()),
                "SM-123"
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotNextVersionException>(() => service.NewRound(newRoundEvent));
        }

        [Fact]
        public async Task SelectCard()
        {
            var events = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                gameId,
                new EventVersion(4),
                new PlayerCard(
                    adminId,
                    cards.Value[0].Id
                )
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.SelectCard(selectCardEvent);

            Assert.Equal(
                new PlayerCardSet(selectCardEvent.PlayerCard),
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
            var events = GetNewGame();
            var selectCardEvent = new SelectCardEvent(
                gameId,
                new EventVersion(2),
                new PlayerCard(
                    adminId,
                    cards.Value[0].Id
                )
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoActiveRoundException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenNoPlayer()
        {
            var events = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                gameId,
                new EventVersion(4),
                new PlayerCard(
                    new UserId(Guid.NewGuid()),
                    cards.Value[0].Id
                )
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoPlayerException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenNoCard()
        {
            var events = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                gameId,
                new EventVersion(4),
                new PlayerCard(
                    adminId,
                    new CardId(Guid.NewGuid())
                )
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoCardException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenNotNextVersion()
        {
            var events = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                gameId,
                new EventVersion(3),
                new PlayerCard(
                    adminId,
                    cards.Value[0].Id
                )
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotNextVersionException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task SelectCard_ThrowWhenPlayerAlreadySelectedCard()
        {
            var events = GetNewGameWithSelectedCards();
            var selectCardEvent = new SelectCardEvent(
                gameId,
                new EventVersion(6),
                new PlayerCard(
                    adminId,
                    cards.Value[0].Id
                )
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<PlayerCardSet.DuplicatePlayerException>(() => service.SelectCard(selectCardEvent));
        }

        [Fact]
        public async Task EndRound()
        {
            var events = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                gameId,
                new EventVersion(6),
                cards.Value[0].Id
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            var actual = await service.EndRound(endRoundEvent);

            Assert.Null(actual.ActiveRound);
            Assert.Equal(
                new Set<CompletedRound>(
                    new CompletedRound[]{
                        new CompletedRound(
                            roundId,
                            roundName,
                            new PlayerCardSet(
                                new PlayerCard(
                                    adminId,
                                    cards.Value[0].Id
                                ),
                                new PlayerCard(
                                    playerId,
                                    cards.Value[0].Id
                                )
                            ),
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
            var events = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                gameId,
                new EventVersion(6),
                new CardId(Guid.NewGuid())
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoCardException>(() => service.EndRound(endRoundEvent));
        }

        [Fact]
        public async Task EndRound_ThrowWhenNotNextVersion()
        {
            var events = GetNewGameWithSelectedCards();
            var endRoundEvent = new EndRoundEvent(
                gameId,
                new EventVersion(5),
                new CardId(Guid.NewGuid())
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotNextVersionException>(() => service.EndRound(endRoundEvent));
        }

        [Fact]
        public async Task EndRound_ThrowWhenNoActiveRound()
        {
            var events = GetNewGameWithPlayers();
            var endRoundEvent = new EndRoundEvent(
                gameId,
                new EventVersion(3),
                cards.Value[0].Id
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events);
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NoActiveRoundException>(() => service.EndRound(endRoundEvent));
        }

        [Fact]
        public async Task EndRound_ThrowWhenNotAllPlayersSelected()
        {
            var events = GetNewGameWithActiveRound();
            var selectCardEvent = new SelectCardEvent(
                gameId,
                new EventVersion(4),
                new PlayerCard(
                    adminId,
                    cards.Value[0].Id
                )
            );
            var endRoundEvent = new EndRoundEvent(
                gameId,
                new EventVersion(5),
                cards.Value[0].Id
            );
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock.Setup(_ => _.ListEvents(gameId)).ReturnsAsync(events.ConcatOne(selectCardEvent));
            var service = new GameService(eventRepositoryMock.Object);

            await Assert.ThrowsAsync<NotAllPlayersSelectedException>(() => service.EndRound(endRoundEvent));
        }

        private IList<Event> GetNewGame()
        {
            return new Event[] {
                new NewEvent(
                    gameId,
                    new EventVersion(1),
                    adminId,
                    cards
                )
            };
        }

        private IList<Event> GetNewGameWithPlayers()
        {
            var events = GetNewGame();
            return events.ConcatOne(new AddPlayerEvent(
                gameId,
                new EventVersion(2),
                playerId
            ));
        }

        private IList<Event> GetNewGameWithActiveRound()
        {
            var events = GetNewGameWithPlayers();
            return events.ConcatOne(new NewRoundEvent(
                gameId,
                new EventVersion(3),
                roundId,
                roundName
            ));
        }

        private IList<Event> GetNewGameWithSelectedCards()
        {
            var events = GetNewGameWithActiveRound();
            return events.ConcatOne(new SelectCardEvent(
                gameId,
                new EventVersion(4),
                new PlayerCard(
                    adminId,
                    cards.Value[0].Id
                )
            )).ConcatOne(new SelectCardEvent(
                gameId,
                new EventVersion(5),
                new PlayerCard(
                    playerId,
                    cards.Value[0].Id
                )
            ));
        }

    }
}
