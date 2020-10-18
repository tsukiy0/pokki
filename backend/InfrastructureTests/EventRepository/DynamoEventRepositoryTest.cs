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
            await using (var fixture = await DynamoEventRepositoryFixture.Init(Environment.GetEnvironmentVariable("DYNAMO_URL")))
            {
                var eventRepository = fixture.GetEventRepository();
            }
        }
    }
}
