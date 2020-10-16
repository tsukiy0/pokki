using System;
using System.Collections.Generic;

namespace Core
{
    public struct EventVersion
    {
        public readonly int Value;

        public EventVersion(int value)
        {
            Value = value;
        }
    }

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

    public class NewGameEvent : Event
    {
        public readonly Person Admin;

        public NewGameEvent(GameId gameId, EventVersion version, Person admin) : base(gameId, version)
        {
            Admin = admin;
        }
    }

    public class AddPersonEvent : Event
    {
        public readonly Person Person;

        public AddPersonEvent(GameId gameId, EventVersion version, Person person) : base(gameId, version)
        {
            Person = person;
        }
    }

    public class AddCardsEvent : Event
    {
        public readonly NonEmptySet<Card> Cards;

        public AddCardsEvent(GameId gameId, EventVersion version, NonEmptySet<Card> cards) : base(gameId, version)
        {
            Cards = cards;
        }
    }

    public class NewRoundEvent : Event
    {
        public readonly RoundId RoundId;

        public NewRoundEvent(GameId gameId, EventVersion version, RoundId roundId) : base(gameId, version)
        {
            RoundId = roundId;
        }
    }

    public class SelectCardEvent : Event
    {
        public readonly PersonCard PersonCard;

        public SelectCardEvent(GameId gameId, EventVersion version, PersonCard personCard) : base(gameId, version)
        {
            PersonCard = personCard;
        }
    }

    public class EndRoundEvent : Event
    {
        public readonly Card Card;

        public EndRoundEvent(GameId gameId, EventVersion version, Card card) : base(gameId, version)
        {
            Card = card;
        }
    }
}
