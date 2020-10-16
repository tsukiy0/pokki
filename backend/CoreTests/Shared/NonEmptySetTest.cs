using System.Collections.Generic;
using Core.Shared;
using Xunit;

namespace CoreTests.Shared
{
    public class NonEmptySetTest
    {
        [Fact]
        public void ThrowWhenNotEmpty()
        {
            Assert.Throws<NotEmptyException>(() => new NonEmptySet<int>(new List<int> { }));
        }
    }
}
