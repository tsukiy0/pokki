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
        public void ThrowWhenNoNew()
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
        public void ThrowWhenMultipleNew()
        {
            var newEvent1 = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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

            var newEvent2 = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid()),
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

            Assert.Throws<MultipleNewException>(() => new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent1,
                newEvent2
            })));
        }

        [Fact]
        public void ThrowWhenNotAscendingEventOrder()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid()),
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

            var addPlayerEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid())
            );

            Assert.Throws<NotAscendingEventOrderException>(() => new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent,
                addPlayerEvent
            })));
        }

        [Fact]
        public void NewEvent()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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

        [Fact]
        public void AddOnePlayerEvent()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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
            var addPlayerEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var actual = new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent,
                addPlayerEvent
            }));

            Assert.Equal(new Game(
                newEvent.GameId,
                addPlayerEvent.Version,
                new NonEmptySet<PlayerRole>(new PlayerRole[] {
                    new PlayerRole(
                        newEvent.AdminId,
                        Role.Admin
                    ),
                    new PlayerRole(
                        addPlayerEvent.PlayerId,
                        Role.Player
                    )
                }),
                newEvent.Cards,
                null,
                new Set<CompletedRound>(new CompletedRound[] { })
            ), actual);
        }

        [Fact]
        public void AddMultiplePlayerEvent()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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
            var addPlayerEvent1 = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var addPlayerEvent2 = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(3),
                new UserId(Guid.NewGuid())
            );
            var actual = new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent,
                addPlayerEvent1,
                addPlayerEvent2
            }));

            Assert.Equal(new Game(
                newEvent.GameId,
                addPlayerEvent2.Version,
                new NonEmptySet<PlayerRole>(new PlayerRole[] {
                    new PlayerRole(
                        newEvent.AdminId,
                        Role.Admin
                    ),
                    new PlayerRole(
                        addPlayerEvent1.PlayerId,
                        Role.Player
                    ),
                    new PlayerRole(
                        addPlayerEvent2.PlayerId,
                        Role.Player
                    )
                }),
                newEvent.Cards,
                null,
                new Set<CompletedRound>(new CompletedRound[] { })
            ), actual);
        }

        [Fact]
        public void ThrowWhenAddPlayerThatExists()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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
            var addPlayerEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                newEvent.AdminId
            );

            Assert.Throws<PlayerConflictException>(() => new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent,
                addPlayerEvent,
            })));
        }


        [Fact]
        public void StartNewRound()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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
            var addPlayerEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var newRoundEvent = new NewRoundEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(3),
                new Round(
                    new RoundId(Guid.NewGuid()),
                    "SM-123",
                    new Set<PlayerCard>(new PlayerCard[] { })
                )
            );
            var actual = new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent,
                addPlayerEvent,
                newRoundEvent
            }));

            Assert.Equal(new Game(
                newEvent.GameId,
                newRoundEvent.Version,
                new NonEmptySet<PlayerRole>(new PlayerRole[] {
                    new PlayerRole(
                        newEvent.AdminId,
                        Role.Admin
                    ),
                    new PlayerRole(
                        addPlayerEvent.PlayerId,
                        Role.Player
                    )
                }),
                newEvent.Cards,
                new Round(
                    newRoundEvent.Round.Id,
                    newRoundEvent.Round.Name,
                    newRoundEvent.Round.PlayerCards
                ),
                new Set<CompletedRound>(new CompletedRound[] { })
            ), actual);
        }

        [Fact]
        public void ThrowWhenSelectCardBeforeNewRound()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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
            var addPlayerEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var selectCardEvent = new SelectCardEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(3),
                new PlayerCard(
                    addPlayerEvent.PlayerId,
                    newEvent.Cards.Value[0].Id
                )
            );

            Assert.Throws<NoActiveRoundException>(() => new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent,
                addPlayerEvent,
                selectCardEvent
            })));
        }

        [Fact]
        public void SelectCardEvent()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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
            var addPlayerEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var newRoundEvent = new NewRoundEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(3),
                new Round(
                    new RoundId(Guid.NewGuid()),
                    "SM-123",
                    new Set<PlayerCard>(new PlayerCard[] { })
                )
            );
            var selectCardEvent = new SelectCardEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(4),
                new PlayerCard(
                    addPlayerEvent.PlayerId,
                    newEvent.Cards.Value[0].Id
                )
            );
            var actual = new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent,
                addPlayerEvent,
                newRoundEvent,
                selectCardEvent
            }));

            Assert.Equal(new Game(
                newEvent.GameId,
                selectCardEvent.Version,
                new NonEmptySet<PlayerRole>(new PlayerRole[] {
                    new PlayerRole(
                        newEvent.AdminId,
                        Role.Admin
                    ),
                    new PlayerRole(
                        addPlayerEvent.PlayerId,
                        Role.Player
                    )
                }),
                newEvent.Cards,
                new Round(
                    newRoundEvent.Round.Id,
                    newRoundEvent.Round.Name,
                    new Set<PlayerCard>(new PlayerCard[]{
                        selectCardEvent.PlayerCard
                    })
                ),
                new Set<CompletedRound>(new CompletedRound[] { })
            ), actual);
        }

        [Fact]
        public void EndRoundEvent()
        {
            var newEvent = new NewEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(1),
                new UserId(Guid.NewGuid()),
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
            var addPlayerEvent = new AddPlayerEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(2),
                new UserId(Guid.NewGuid())
            );
            var newRoundEvent = new NewRoundEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(3),
                new Round(
                    new RoundId(Guid.NewGuid()),
                    "SM-123",
                    new Set<PlayerCard>(new PlayerCard[] { })
                )
            );
            var selectCardEvent1 = new SelectCardEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(4),
                new PlayerCard(
                    addPlayerEvent.PlayerId,
                    newEvent.Cards.Value[0].Id
                )
            );
            var selectCardEvent2 = new SelectCardEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(5),
                new PlayerCard(
                    newEvent.AdminId,
                    newEvent.Cards.Value[0].Id
                )
            );
            var endRoundEvent = new EndRoundEvent(
                new GameId(Guid.NewGuid()),
                new EventVersion(6),
                newEvent.Cards.Value[0].Id
            );
            var actual = new EventReducer().Reduce(new NonEmptySet<Event>(new Event[]{
                newEvent,
                addPlayerEvent,
                newRoundEvent,
                selectCardEvent1,
                selectCardEvent2,
                endRoundEvent
            }));

            Assert.Equal(new Game(
                newEvent.GameId,
                endRoundEvent.Version,
                new NonEmptySet<PlayerRole>(new PlayerRole[] {
                    new PlayerRole(
                        newEvent.AdminId,
                        Role.Admin
                    ),
                    new PlayerRole(
                        addPlayerEvent.PlayerId,
                        Role.Player
                    )
                }),
                newEvent.Cards,
                null,
                new Set<CompletedRound>(new CompletedRound[] {
                    new CompletedRound(
                        newRoundEvent.Round.Id,
                        newRoundEvent.Round.Name,
                        new NonEmptySet<PlayerCard>(new PlayerCard[]{
                            selectCardEvent1.PlayerCard,
                            selectCardEvent2.PlayerCard
                        }),
                        endRoundEvent.ResultCardId
                    )
                })
            ), actual);
        }
    }
}
