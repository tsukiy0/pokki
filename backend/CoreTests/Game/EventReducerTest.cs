using System;
using Core.Game;
using Core.Shared;
using Core.User;
using Xunit;

namespace CoreTests
{
    public class EventReducerTest
    {
        [Fact]
        public void ThrowWhenNoNewGame()
        {
            Assert.Throws<NoNewGameException>(() => new EventReducer().Reduce(new NonEmptySet<Event>(new Event[] {
                new AddPlayerEvent(
                    new GameId(Guid.NewGuid()),
                    new EventVersion(1),
                    new UserId(Guid.NewGuid())
                )
            })));
        }

        [Fact]
        public void NewGame()
        {
            var adminId = new UserId(Guid.NewGuid());
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                adminId,
                new NonEmptySet<Card>(new Card[] {
                    new Card(
                        new CardId(Guid.NewGuid()),
                        "M"
                    ),
                    new Card(
                        new CardId(Guid.NewGuid()),
                        "L"
                    )
                })
            );
            var actual = new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent
            }));

            Assert.Equal(new Game(
                newEvent.GameId,
                newEvent.Version,
                new NonEmptySet<PlayerRole>(new PlayerRole[] {
                    new PlayerRole(
                        newEvent.AdminId,
                        Role.Admin
                    )
                }),
                newEvent.Cards,
                null,
                new Set<CompletedRound>(new CompletedRound[] { })
            ), actual);
        }

        // [Fact]
        // public void ThrowWhenCardsAlreadyExist()
        // {
        //     var gameId = new GameId(Guid.NewGuid());

        //     Assert.Throws<CardsAlreadyExistException>(() => new GameEventReducer().Reduce(new NonEmptySet<Event>(new List<Event> {
        //         new NewGameGameEvent(
        //             gameId,
        //             new GameEventVersion(1),
        //             new Person(
        //                 new PersonId(Guid.NewGuid()),
        //                 "bob"
        //             )
        //         ),
        //         new AddCardsGameEvent(
        //             gameId,
        //             new GameEventVersion(2),
        //             new NonEmptySet<Card>(new List<Card>{ new Card(new CardId(Guid.NewGuid()), "card1"), new Card(new CardId(Guid.NewGuid()), "card2")})
        //         ),
        //         new AddCardsGameEvent(
        //             gameId,
        //             new GameEventVersion(3),
        //             new NonEmptySet<Card>(new List<Card>{ new Card(new CardId(Guid.NewGuid()), "card1"), new Card(new CardId(Guid.NewGuid()), "card2")})
        //         )
        //     })));
        // }

        // [Fact]
        // public void ThrowWhenRoundAlreadyStarted()
        // {
        //     var gameId = new GameId(Guid.NewGuid());

        //     Assert.Throws<CardsAlreadyExistException>(() => new GameEventReducer().Reduce(new NonEmptySet<Event>(new List<Event> {
        //         new NewGameGameEvent(
        //             gameId,
        //             new GameEventVersion(1),
        //             new Person(
        //                 new PersonId(Guid.NewGuid()),
        //                 "bob"
        //             )
        //         ),
        //         new AddCardsGameEvent(
        //             gameId,
        //             new GameEventVersion(2),
        //             new NonEmptySet<Card>(new List<Card>{ new Card(new CardId(Guid.NewGuid()), "card1"), new Card(new CardId(Guid.NewGuid()), "card2")})
        //         ),
        //         new AddCardsGameEvent(
        //             gameId,
        //             new GameEventVersion(3),
        //             new NonEmptySet<Card>(new List<Card>{ new Card(new CardId(Guid.NewGuid()), "card1"), new Card(new CardId(Guid.NewGuid()), "card2")})
        //         )
        //     })));
        // }
    }
}
