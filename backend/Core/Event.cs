using System;
using System.Collections.Generic;

namespace Core
{
    public struct GameId
    {
        public readonly Guid Value;

        public GameId(Guid value)
        {
            Value = value;
        }
    }

    public struct EventVersion
    {
        public readonly int Value;

        public EventVersion(int value)
        {
            Value = value;
        }
    }

    public struct Event
    {
        public readonly GameId GameId;
        public readonly EventVersion Version;

        public Event(GameId gameId, EventVersion version)
        {
            GameId = gameId;
            Version = version;
        }
    }

    public struct NewGameEvent
    {
        public readonly Event Event;
        public readonly Person Admin;

        public NewGameEvent(Event @event, Person admin)
        {
            Event = @event;
            Admin = admin;
        }
    }

    public struct AddPersonEvent
    {
        public readonly Event Event;
        public readonly Person Person;

        public AddPersonEvent(Event @event, Person person)
        {
            Event = @event;
            Person = person;
        }
    }

    public struct AddCardsEvent
    {
        public readonly Event Event;
        public readonly IList<Card> Cards;

        public AddCardsEvent(Event @event, IList<Card> cards)
        {
            Event = @event;
            Cards = cards;
        }
    }

    public struct NewRoundEvent
    {
        public readonly Event Event;
        public readonly RoundId RoundId;

        public NewRoundEvent(Event @event, RoundId roundId)
        {
            Event = @event;
            RoundId = roundId;
        }
    }
}
