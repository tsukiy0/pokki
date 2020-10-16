using System;
using System.Collections.Generic;
using Core;
using Xunit;

namespace CoreTests
{
    public class EventReducerTest
    {
        [Fact]
        public void ThrowWhenFirstEventNotNewGame()
        {
            Assert.Throws<NoNewGameEventException>(() => new NonEmptySet<Event>(new List<Event> {
                new AddPersonEvent(
                    new GameId(Guid.NewGuid()),
                    new EventVersion(1),
                    new Person(
                        new PersonId(Guid.NewGuid()),
                        "bob"
                    )
                )
            }));
        }
    }
}
