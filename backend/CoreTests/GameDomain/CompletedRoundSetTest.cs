using System;
using Core.GameDomain;
using Xunit;

namespace GameTests
{
    [Trait("Category", "Unit")]
    public class CompletedRoundSetTest
    {
        [Fact]
        public void ThrowWhenDuplicateRound()
        {
            var roundId = new RoundId(Guid.NewGuid());
            Assert.Throws<CompletedRoundSet.DuplicateRoundException>(() => new CompletedRoundSet(
                new CompletedRound(
                    roundId,
                    "L",
                    new PlayerCardSet(),
                    new CardId(Guid.NewGuid())
                ),
                new CompletedRound(
                    roundId,
                    "M",
                    new PlayerCardSet(),
                    new CardId(Guid.NewGuid())
                )
            ));
        }

        [Fact]
        public void AddRound()
        {
            var round = new CompletedRound(
                new RoundId(Guid.NewGuid()),
                "L",
                new PlayerCardSet(),
                new CardId(Guid.NewGuid())
            );

            var actual = new CompletedRoundSet().AddRound(round);

            Assert.Equal(new CompletedRoundSet(round), actual);
        }
    }
}
