using System;
using Core.Game;
using Core.Shared;
using Core.User;
using Xunit;

namespace CoreTests
{
    public class GameEventReducerTest
    {
        [Fact]
        public void ThrowWhenNoNewGame()
        {
            Assert.Throws<NoNewGameException>(() => new GameEventReducer().Reduce(new NonEmptySet<GameEvent>(new GameEvent[] {
                new AddPlayerGameEvent(
                    new GameId(Guid.NewGuid()),
                    new GameEventVersion(1),
                    new UserId(Guid.NewGuid())
                )
            })));
        }

        [Fact]
        public void NewGame()
        {
            var adminId = new UserId(Guid.NewGuid());
            var newGameEvent = new NewGameEvent(
                new GameId(Guid.NewGuid()),
                new GameEventVersion(1),
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
            var actual = new GameEventReducer().Reduce(new NonEmptySet<GameEvent>(new GameEvent[]{
                newGameEvent
            }));

            Assert.Equal(new Game(
                newGameEvent.GameId,
                newGameEvent.Version,
                new NonEmptySet<PlayerRole>(new PlayerRole[] {
                    new PlayerRole(
                        newGameEvent.AdminId,
                        Role.Admin
                    )
                }),
                newGameEvent.Cards,
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
