using System;

namespace Core
{
    public class GameId : Guid { }

    public class EventVersion
    {
        private readonly int _value;

        public EventVersion(int value)
        {
            _value = value;
        }

        public readonly int ToInt()
        {
            return _value;
        }
    }

    public sealed abstract class Event
    {
        public readonly GameId GameId;
        public readonly EventVersion Version;

        public Event(GameId gameId, EventVersion version)
        {
            GameId = gameId;
            Version = version;
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
}
