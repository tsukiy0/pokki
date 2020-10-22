using System.Text.Json;
using Xunit;

namespace ApiTests
{
    [Trait("Category", "Unit")]
    public class GraphQlRequestTest
    {
        [Fact]
        public void ParentType_Query()
        {
            var actual = JsonSerializer.Deserialize<Api.Function.GraphQlRequestParentType>("\"Query\"");

            Assert.Equal(Api.Function.GraphQlRequestParentType.Query, actual);
        }

        [Fact]
        public void ParentType_Mutation()
        {
            var actual = JsonSerializer.Deserialize<Api.Function.GraphQlRequestParentType>("\"Mutation\"");

            Assert.Equal(Api.Function.GraphQlRequestParentType.Mutation, actual);
        }
    }
}
