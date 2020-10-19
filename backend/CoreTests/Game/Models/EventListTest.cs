
using Core.Game.Models;
using Core.Shared;
using Core.User;
using System;
using Xunit;

namespace CoreTests
{
    [Trait("Category", "Unit")]
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

        [Fact]
        public void Reduce()
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
                new RoundId(Guid.NewGuid()),
                "SM-123"
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

            var actual = new EventList(new Event[]{
                newEvent,
                addPlayerEvent,
                newRoundEvent,
                selectCardEvent1,
                selectCardEvent2,
                endRoundEvent
            }).Reduce();

            Assert.Equal(newEvent.GameId, actual.Id);
            Assert.Equal(endRoundEvent.Version, actual.Version);
            Assert.Equal(new NonEmptySet<PlayerRole>(new PlayerRole[] {
                new PlayerRole(
                    newEvent.AdminId,
                    Role.Admin
                ),
                new PlayerRole(
                    addPlayerEvent.PlayerId,
                    Role.Player
                )
            }), actual.PlayerRoles);
            Assert.Equal(newEvent.Cards, actual.Cards);
            Assert.Null(actual.ActiveRound);
            Assert.Equal(new Set<CompletedRound>(new CompletedRound[] {
                new CompletedRound(
                    newRoundEvent.RoundId,
                    newRoundEvent.RoundName,
                    new NonEmptySet<PlayerCard>(new PlayerCard[]{
                        selectCardEvent1.PlayerCard,
                        selectCardEvent2.PlayerCard
                    }),
                    endRoundEvent.ResultCardId
                )
            }), actual.CompletedRounds);
        }
    }
}
