using System;
using System.Linq;
using Core.Shared;
using Core.User;

namespace Core.Game
{
    public struct GameId
    {
        public readonly Guid Value;

        public GameId(Guid value)
        {
            Value = value;
        }
    }

    public class NoActiveRoundException : Exception { }
    public class PlayerConflictException : Exception { }
    public class ActiveRoundConflictException : Exception { }
    public class NoPlayerException : Exception { }
    public class NoCardException : Exception { }
    public class NotAllPlayersSelectedException : Exception { }
    public class PlayerCardConflictException : Exception { }
    public class NotNextVersionException : Exception { }

    public struct Game
    {
        public readonly GameId Id;
        public readonly EventVersion Version;
        public readonly NonEmptySet<PlayerRole> PlayerRoles;
        public readonly NonEmptySet<Card> Cards;
        public readonly Round? ActiveRound;
        public readonly Set<CompletedRound> CompletedRounds;

        private Game(GameId id, EventVersion version, NonEmptySet<PlayerRole> playerRoles, NonEmptySet<Card> cards, Round? activeRound, Set<CompletedRound> completedRounds)
        {
            Id = id;
            Version = version;
            PlayerRoles = playerRoles;
            Cards = cards;
            ActiveRound = activeRound;
            CompletedRounds = completedRounds;
        }

        public static Game New(NewEvent @event)
        {
            return new Game(
                @event.GameId,
                @event.Version,
                new NonEmptySet<PlayerRole>(new PlayerRole[] {
                    new PlayerRole(
                        @event.AdminId,
                        Role.Admin
                    )
                }),
                @event.Cards,
                null,
                new Set<CompletedRound>(Array.Empty<CompletedRound>())
            );
        }

        public Game AddNewPlayer(AddPlayerEvent @event)
        {
            if (!IsNextVersion(@event.Version))
            {
                throw new NotNextVersionException();
            }

            if (HasPlayer(@event.PlayerId))
            {
                throw new PlayerConflictException();
            }

            return new Game(
                Id,
                @event.Version,
                PlayerRoles.ConcatOne(
                    new PlayerRole(
                        @event.PlayerId,
                        Role.Player
                    )
                ),
                Cards,
                ActiveRound,
                CompletedRounds
            );
        }

        public Game NewRound(NewRoundEvent @event)
        {
            if (!IsNextVersion(@event.Version))
            {
                throw new NotNextVersionException();
            }

            if (ActiveRound != null)
            {
                throw new ActiveRoundConflictException();
            }

            return new Game(
                Id,
                @event.Version,
                PlayerRoles,
                Cards,
                new Round(
                    @event.RoundId,
                    @event.RoundName,
                    new Set<PlayerCard>(Array.Empty<PlayerCard>())
                ),
                CompletedRounds
            );
        }

        public Game SelectCard(SelectCardEvent @event)
        {
            if (!IsNextVersion(@event.Version))
            {
                throw new NotNextVersionException();
            }

            if (ActiveRound == null)
            {
                throw new NoActiveRoundException();
            }

            if (!HasCard(@event.PlayerCard.CardId))
            {
                throw new NoCardException();
            }

            if (!HasPlayer(@event.PlayerCard.PlayerId))
            {
                throw new NoPlayerException();
            }

            if (HasPlayerCard(@event.PlayerCard.PlayerId))
            {
                throw new PlayerCardConflictException();
            }


            return new Game(
                Id,
                @event.Version,
                PlayerRoles,
                Cards,
                new Round(
                    ActiveRound.Value.Id,
                    ActiveRound.Value.Name,
                    ActiveRound.Value.PlayerCards.ConcatOne(@event.PlayerCard)
                ),
                CompletedRounds
            );
        }

        public Game EndRound(EndRoundEvent @event)
        {
            if (!IsNextVersion(@event.Version))
            {
                throw new NotNextVersionException();
            }

            if (ActiveRound == null)
            {
                throw new NoActiveRoundException();
            }

            if (!HasCard(@event.ResultCardId))
            {
                throw new NoCardException();
            }

            if (!HasAllPlayersSelected())
            {
                throw new NotAllPlayersSelectedException();
            }

            return new Game(
                Id,
                @event.Version,
                PlayerRoles,
                Cards,
                null,
                CompletedRounds.ConcatOne(
                    new CompletedRound(
                        ActiveRound.Value.Id,
                        ActiveRound.Value.Name,
                        new NonEmptySet<PlayerCard>(ActiveRound.Value.PlayerCards.Value),
                        @event.ResultCardId
                    )
                )
            );
        }

        public UserId GetAdminId()
        {
            return PlayerRoles.Value
                .Where(_ => _.Role == Role.Admin)
                .Single().PlayerId;
        }

        private bool HasPlayer(UserId playerId)
        {
            return PlayerRoles.Value
                .Where(_ => _.PlayerId.Equals(playerId))
                .Count() == 1;
        }

        private bool HasPlayerCard(UserId playerId)
        {
            if (ActiveRound == null)
            {
                return false;
            }

            return ActiveRound.Value
                .PlayerCards.Value
                .Where(_ => _.PlayerId.Equals(playerId))
                .Count() == 1;
        }

        private bool HasCard(CardId cardId)
        {
            return Cards.Value
                .Where(_ => _.Id.Equals(cardId))
                .Count() == 1;
        }

        private bool HasAllPlayersSelected()
        {
            if (ActiveRound == null)
            {
                return false;
            }

            return !PlayerRoles.Value
                .Select(_ => _.PlayerId)
                .Except(ActiveRound.Value.PlayerCards.Value.Select(_ => _.PlayerId))
                .Any();
        }

        private bool IsNextVersion(EventVersion version)
        {
            return version.Value == Version.Value + 1;
        }
    }
}
