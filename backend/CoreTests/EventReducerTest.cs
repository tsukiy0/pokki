using System;
using System.Collections.Generic;
using Core;
using Xunit;

namespace CoreTests
{
    public class EventReducerTest
    {
        // [Fact]
        // public void ThrowWhenGameNotStarted()
        // {
        //     Assert.Throws<GameNotStartedException>(() => new EventReducer().Reduce(new NonEmptySet<Event>(new List<Event> {
        //         new AddPersonEvent(
        //             new GameId(Guid.NewGuid()),
        //             new EventVersion(1),
        //             new Person(
        //                 new PersonId(Guid.NewGuid()),
        //                 "bob"
        //             )
        //         )
        //     })));
        // }

        // [Fact]
        // public void ThrowWhenCardsAlreadyExist()
        // {
        //     var gameId = new GameId(Guid.NewGuid());

        //     Assert.Throws<CardsAlreadyExistException>(() => new EventReducer().Reduce(new NonEmptySet<Event>(new List<Event> {
        //         new NewGameEvent(
        //             gameId,
        //             new EventVersion(1),
        //             new Person(
        //                 new PersonId(Guid.NewGuid()),
        //                 "bob"
        //             )
        //         ),
        //         new AddCardsEvent(
        //             gameId,
        //             new EventVersion(2),
        //             new NonEmptySet<Card>(new List<Card>{ new Card(new CardId(Guid.NewGuid()), "card1"), new Card(new CardId(Guid.NewGuid()), "card2")})
        //         ),
        //         new AddCardsEvent(
        //             gameId,
        //             new EventVersion(3),
        //             new NonEmptySet<Card>(new List<Card>{ new Card(new CardId(Guid.NewGuid()), "card1"), new Card(new CardId(Guid.NewGuid()), "card2")})
        //         )
        //     })));
        // }

        // [Fact]
        // public void ThrowWhenRoundAlreadyStarted()
        // {
        //     var gameId = new GameId(Guid.NewGuid());

        //     Assert.Throws<CardsAlreadyExistException>(() => new EventReducer().Reduce(new NonEmptySet<Event>(new List<Event> {
        //         new NewGameEvent(
        //             gameId,
        //             new EventVersion(1),
        //             new Person(
        //                 new PersonId(Guid.NewGuid()),
        //                 "bob"
        //             )
        //         ),
        //         new AddCardsEvent(
        //             gameId,
        //             new EventVersion(2),
        //             new NonEmptySet<Card>(new List<Card>{ new Card(new CardId(Guid.NewGuid()), "card1"), new Card(new CardId(Guid.NewGuid()), "card2")})
        //         ),
        //         new AddCardsEvent(
        //             gameId,
        //             new EventVersion(3),
        //             new NonEmptySet<Card>(new List<Card>{ new Card(new CardId(Guid.NewGuid()), "card1"), new Card(new CardId(Guid.NewGuid()), "card2")})
        //         )
        //     })));
        // }
    }
}
