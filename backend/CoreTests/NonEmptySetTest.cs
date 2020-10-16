using System.Collections.Generic;
using Core;
using Xunit;

namespace CoreTests
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
