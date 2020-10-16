using System.Collections.Generic;
using Core.Shared;
using Xunit;

namespace CoreTests.Shared
{
    public class SetTest
    {
        [Fact]
        public void ThrowWhenDuplicate()
        {
            Assert.Throws<DuplicateException>(() => new Set<int>(new List<int> { 1, 1 }));
        }
    }
}
