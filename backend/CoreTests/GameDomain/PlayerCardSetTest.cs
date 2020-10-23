using System;
using Core.GameDomain;
using Core.UserDomain;
using Xunit;

namespace GameTests
{
    [Trait("Category", "Unit")]
    public class PlayerCardSetTest
    {
        [Fact]
        public void ThrowWhenDuplicatePlayer()
        {
            var playerId = new UserId(Guid.NewGuid());
            Assert.Throws<PlayerCardSet.DuplicatePlayerException>(() => new PlayerCardSet(
                new PlayerCard(
                    playerId,
                    new CardId(Guid.NewGuid())
                ),
                new PlayerCard(
                    playerId,
                    new CardId(Guid.NewGuid())
                )
            ));
        }

        [Fact]
        public void AddPlayerCard()
        {
            var playerCard = new PlayerCard(
                new UserId(Guid.NewGuid()),
                new CardId(Guid.NewGuid())
            );
            var set = new PlayerCardSet().AddPlayerCard(playerCard);

            Assert.Equal(new PlayerCardSet(playerCard), set);
        }
    }
}
