using Core.GameDomain;
using Core.UserDomain;
using Infrastructure.GameDomain;
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
                    new UserId(Guid.NewGuid()),
                    new CardSet(
                        new Card(
                            new CardId(Guid.NewGuid()),
                            "M"
                        )
                    )
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
                    new CardId(Guid.NewGuid())
                );

                await eventRepository.AppendEvent(@event);

                var events = await eventRepository.ListEvents(@event.GameId);

                Assert.Equal(@event, events[0]);
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
                    new UserId(Guid.NewGuid()),
                    new CardSet(
                        new Card(
                            new CardId(Guid.NewGuid()),
                            "M"
                        )
                    )
                );

                var addPlayerEvent = new AddPlayerEvent(
                    gameId,
                    new UserId(Guid.NewGuid())
                );

                var newRoundEvent = new NewRoundEvent(
                    gameId,
                    new RoundId(Guid.NewGuid()),
                    "SM-123"
                );

                var selectCardEvent = new SelectCardEvent(
                    gameId,
                    new PlayerCard(
                        new UserId(Guid.NewGuid()),
                        new CardId(Guid.NewGuid())
                    )
                );

                var endRoundEvent = new EndRoundEvent(
                    gameId,
                    new CardId(Guid.NewGuid())
                );

                await eventRepository.AppendEvent(newEvent);
                await eventRepository.AppendEvent(addPlayerEvent);
                await eventRepository.AppendEvent(newRoundEvent);
                await eventRepository.AppendEvent(selectCardEvent);
                await eventRepository.AppendEvent(endRoundEvent);

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
