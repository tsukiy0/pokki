using Core.Shared;
using System.Collections.Generic;
using Xunit;

namespace CoreTests.Shared
{
    [Trait("Category", "Unit")]
    public class SetTest
    {
        [Fact]
        public void ThrowWhenDuplicate()
        {
            Assert.Throws<DuplicateException>(() => new Set<int>(new List<int> { 1, 1 }));
        }
    }
}
