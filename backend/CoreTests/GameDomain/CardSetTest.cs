using System;
using Core.GameDomain;
using Xunit;

namespace GameTests
{
    [Trait("Category", "Unit")]
    public class CardSetTest
    {
        [Fact]
        public void ThrowWhenDuplicateCard()
        {
            var cardId = new CardId(Guid.NewGuid());
            Assert.Throws<CardSet.DuplicateCardException>(() => new CardSet(
                new Card(
                    cardId,
                    "L"
                ),
                new Card(
                    cardId,
                    "M"
                )
            ));
        }

        [Fact]
        public void ThrowWhenDuplicateCardName()
        {
            var cardId = new CardId(Guid.NewGuid());
            Assert.Throws<CardSet.DuplicateCardNameException>(() => new CardSet(
                new Card(
                    new CardId(Guid.NewGuid()),
                    "M"
                ),
                new Card(
                    new CardId(Guid.NewGuid()),
                    "M"
                )
            ));
        }

        [Fact]
        public void AddCard()
        {
            var card = new Card(
                new CardId(Guid.NewGuid()),
                "M"
            );
            var set = new CardSet().AddCard(card);

            Assert.Equal(new CardSet(card), set);
        }
    }
}
