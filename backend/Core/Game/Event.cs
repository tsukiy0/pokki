using System;
using System.Collections.Generic;
using Core.Shared;
using Core.User;

namespace Core.Game
{
    public abstract class Event
    {
        public readonly GameId GameId;
        public readonly EventVersion Version;

        public Event(GameId gameId, EventVersion version)
        {
            GameId = gameId;
            Version = version;
        }

        public override bool Equals(object? obj)
        {
            return obj is Event @event &&
                   EqualityComparer<GameId>.Default.Equals(GameId, @event.GameId) &&
                   EqualityComparer<EventVersion>.Default.Equals(Version, @event.Version);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GameId, Version);
        }
    }

    public class NewEvent : Event
    {
        public readonly UserId AdminId;
        public readonly NonEmptySet<Card> Cards;

        public NewEvent(GameId gameId, EventVersion version, UserId adminId, NonEmptySet<Card> cards) : base(gameId, version)
        {
            AdminId = adminId;
            Cards = cards;
        }
    }

    public class AddPlayerEvent : Event
    {
        public readonly UserId PlayerId;

        public AddPlayerEvent(GameId gameId, EventVersion version, UserId playerId) : base(gameId, version)
        {
            PlayerId = playerId;
        }
    }

    public class StartEvent : Event
    {
        public StartEvent(GameId gameId, EventVersion version) : base(gameId, version)
        { }
    }

    public class NewRoundEvent : Event
    {
        public readonly Round Round;

        public NewRoundEvent(GameId gameId, EventVersion version, Round round) : base(gameId, version)
        {
            Round = round;
        }
    }

    public class SelectCardEvent : Event
    {
        public readonly PlayerCard PlayerCard;

        public SelectCardEvent(GameId gameId, EventVersion version, PlayerCard playerCard) : base(gameId, version)
        {
            PlayerCard = playerCard;
        }
    }

    public class EndRoundEvent : Event
    {
        public readonly CardId ResultCardId;

        public EndRoundEvent(GameId gameId, EventVersion version, CardId resultCardId) : base(gameId, version)
        {
            ResultCardId = resultCardId;
        }
    }
}
