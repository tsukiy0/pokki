using System.Collections.Generic;
using Core;
using Xunit;

namespace CoreTests
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
