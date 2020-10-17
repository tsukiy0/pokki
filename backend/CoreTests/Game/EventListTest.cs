
using System;
using Core.Game;
using Core.Shared;
using Core.User;
using Xunit;

namespace CoreTests
{
    public class EventListTest
    {
        [Fact]
        public void ThrowWhenNewEventNotFirst()
        {
            Assert.Throws<NoNewException>(() => new EventList(
                new Event[]{
                    new AddPlayerEvent(
                        new GameId(Guid.NewGuid()),
                        new EventVersion(1),
                        new UserId(Guid.NewGuid())
                    )
                }
            ));
        }

        [Fact]
        public void ThrowWhenMultipleNewEvents()
        {
            Assert.Throws<MultipleNewException>(() => new EventList(
                new Event[]{
                    new NewEvent(
                        new GameId(Guid.NewGuid()),
                        new EventVersion(1),
                        new UserId(Guid.NewGuid()),
                        new NonEmptySet<Card>(new Card[]{
                            new Card(
                                new CardId(Guid.NewGuid()),
                                "M"
                            )
                        })
                    ),
                    new NewEvent(
                        new GameId(Guid.NewGuid()),
                        new EventVersion(2),
                        new UserId(Guid.NewGuid()),
                        new NonEmptySet<Card>(new Card[]{
                            new Card(
                                new CardId(Guid.NewGuid()),
                                "L"
                            )
                        })
                    ),
                }
            ));
        }
    }
}
