using Core.GameDomain;
using Core.Shared;
using Core.UserDomain;
using Infrastructure.GameDomain.EventRepository;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InfrastructureTests
{
    [Trait("Category", "Contract")]
    public class DynamoEventRepositoryTest
    {
        [Fact]
        public async Task AppendEvent_New()
        {
            await using (var fixture = await DynamoEventRepositoryFixture.Init())
            {
                var eventRepository = fixture.GetEventRepository();

                var @event = new NewEvent(
                    new GameId(Guid.NewGuid()),
                    new EventVersion(1),
                    new UserId(Guid.NewGuid()),
                    new NonEmptySet<Card>(new Card[]{
                        new Card(
                            new CardId(Guid.NewGuid()),
                            "M"
                        )
                    })
                );

                await eventRepository.AppendEvent(@event);

                var events = await eventRepository.ListEvents(@event.GameId);

                Assert.Equal(@event, events[0]);
            }
        }

        [Fact]
        public async Task AppendEvent_AddPlayer()
        {
            await using (var fixture = await DynamoEventRepositoryFixture.Init())
            {
                var eventRepository = fixture.GetEventRepository();

                var @event = new AddPlayerEvent(
                    new GameId(Guid.NewGuid()),
                    new EventVersion(1),
                    new UserId(Guid.NewGuid())
                );

                await eventRepository.AppendEvent(@event);

                var events = await eventRepository.ListEvents(@event.GameId);

                Assert.Equal(@event, events[0]);
            }
        }

        [Fact]
        public async Task AppendEvent_NewRound()
        {
            await using (var fixture = await DynamoEventRepositoryFixture.Init())
            {
                var eventRepository = fixture.GetEventRepository();

                var @event = new NewRoundEvent(
                    new GameId(Guid.NewGuid()),
                    new EventVersion(1),
                    new RoundId(Guid.NewGuid()),
                    "SM-123"
                );

                await eventRepository.AppendEvent(@event);

                var events = await eventRepository.ListEvents(@event.GameId);

                Assert.Equal(@event, events[0]);
            }
        }

        [Fact]
        public async Task AppendEvent_SelectCardEvent()
        {
            await using (var fixture = await DynamoEventRepositoryFixture.Init())
            {
                var eventRepository = fixture.GetEventRepository();

                var @event = new SelectCardEvent(
                    new GameId(Guid.NewGuid()),
                    new EventVersion(1),
                    new PlayerCard(
                        new UserId(Guid.NewGuid()),
                        new CardId(Guid.NewGuid())
                    )
                );

                await eventRepository.AppendEvent(@event);

                var events = await eventRepository.ListEvents(@event.GameId);

                Assert.Equal(@event, events[0]);
            }
        }

        [Fact]
        public async Task AppendEvent_EndRound()
        {
            await using (var fixture = await DynamoEventRepositoryFixture.Init())
            {
                var eventRepository = fixture.GetEventRepository();

                var @event = new EndRoundEvent(
                    new GameId(Guid.NewGuid()),
                    new EventVersion(1),
                    new CardId(Guid.NewGuid())
                );

                await eventRepository.AppendEvent(@event);

                var events = await eventRepository.ListEvents(@event.GameId);

                Assert.Equal(@event, events[0]);
            }
        }


        [Fact]
        public async Task AppendEvent_ThrowWhenConflictingVersion()
        {
            await using (var fixture = await DynamoEventRepositoryFixture.Init())
            {
                var eventRepository = fixture.GetEventRepository();

                var @event = new EndRoundEvent(
                    new GameId(Guid.NewGuid()),
                    new EventVersion(1),
                    new CardId(Guid.NewGuid())
                );

                await eventRepository.AppendEvent(@event);
                await Assert.ThrowsAsync<DynamoEventRepository.ConflictingVersionException>(() => eventRepository.AppendEvent(@event));
            }
        }

        [Fact]
        public async Task ListEvents_SortByEventVersionAscending()
        {
            await using (var fixture = await DynamoEventRepositoryFixture.Init())
            {
                var eventRepository = fixture.GetEventRepository();

                var gameId = new GameId(Guid.NewGuid());

                var newEvent = new NewEvent(
                    gameId,
                    new EventVersion(1),
                    new UserId(Guid.NewGuid()),
                    new NonEmptySet<Card>(new Card[]{
                        new Card(
                            new CardId(Guid.NewGuid()),
                            "M"
                        )
                    })
                );

                var addPlayerEvent = new AddPlayerEvent(
                    gameId,
                    new EventVersion(2),
                    new UserId(Guid.NewGuid())
                );

                var newRoundEvent = new NewRoundEvent(
                    gameId,
                    new EventVersion(3),
                    new RoundId(Guid.NewGuid()),
                    "SM-123"
                );

                var selectCardEvent = new SelectCardEvent(
                    gameId,
                    new EventVersion(4),
                    new PlayerCard(
                        new UserId(Guid.NewGuid()),
                        new CardId(Guid.NewGuid())
                    )
                );

                var endRoundEvent = new EndRoundEvent(
                    gameId,
                    new EventVersion(5),
                    new CardId(Guid.NewGuid())
                );

                await eventRepository.AppendEvent(newEvent);
                await eventRepository.AppendEvent(endRoundEvent);
                await eventRepository.AppendEvent(selectCardEvent);
                await eventRepository.AppendEvent(addPlayerEvent);
                await eventRepository.AppendEvent(newRoundEvent);

                var events = await eventRepository.ListEvents(gameId);

                Assert.Equal(newEvent, events[0]);
                Assert.Equal(addPlayerEvent, events[1]);
                Assert.Equal(newRoundEvent, events[2]);
                Assert.Equal(selectCardEvent, events[3]);
                Assert.Equal(endRoundEvent, events[4]);
            }
        }
    }
}
