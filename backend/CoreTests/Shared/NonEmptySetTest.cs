using Core.Shared;
using System.Collections.Generic;
using Xunit;

namespace CoreTests.Shared
{
    [Trait("Category", "Unit")]
    public class NonEmptySetTest
    {
        [Fact]
        public void ThrowWhenNotEmpty()
        {
            Assert.Throws<NotEmptyException>(() => new NonEmptySet<int>(new List<int> { }));
        }
    }
}
