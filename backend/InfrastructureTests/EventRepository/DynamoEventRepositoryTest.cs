using Core.Game;
using Core.Shared;
using Core.User;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InfrastructureTests
{
    [Trait("Category", "Contract")]
    public class DynamoEventRepositoryTest
    {
        [Fact]
        public async Task Test1()
        {
            // await using (var fixture = await DynamoEventRepositoryFixture.Init("https://localhost:8000"))
            // {
            var fixture = await DynamoEventRepositoryFixture.Init("http://localhost:8000");
            var eventRepository = fixture.GetEventRepository();

            var newEvent = new NewEvent(
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

            await eventRepository.AppendNewEvent(newEvent);

            var events = await eventRepository.ListEvents(newEvent.GameId);

            Assert.Equal(newEvent, events[0]);
        }
    }
}
