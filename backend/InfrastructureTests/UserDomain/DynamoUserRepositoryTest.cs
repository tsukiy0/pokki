using Core.UserDomain;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InfrastructureTests
{
    [Trait("Category", "Contract")]
    public class DynamoUserRepositoryTest
    {
        [Fact]
        public async Task Create()
        {
            await using (var fixture = await DynamoUserRepositoryFixture.Init())
            {
                var userRepository = fixture.GetUserRepository();

                var user = new User(
                    new UserId(Guid.NewGuid()),
                    "bob"
                );

                await userRepository.CreateUser(user);

                var actual = await userRepository.GetUser(user.Id);

                Assert.Equal(user, actual);
            }
        }
    }
}
