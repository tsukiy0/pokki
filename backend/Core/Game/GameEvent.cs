using System;
using System.Collections.Generic;
using Core.Shared;
using Core.User;

namespace Core.Game
{
    public abstract class GameEvent
    {
        public readonly GameId GameId;
        public readonly EventVersion Version;

        public GameEvent(GameId gameId, EventVersion version)
        {
            GameId = gameId;
            Version = version;
        }

        public override bool Equals(object? obj)
        {
            return obj is GameEvent @event &&
                   EqualityComparer<GameId>.Default.Equals(GameId, @event.GameId) &&
                   EqualityComparer<EventVersion>.Default.Equals(Version, @event.Version);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GameId, Version);
        }
    }

    public class NewLobbyGameEvent : GameEvent
    {
        public readonly UserId AdminId;
        public readonly NonEmptySet<Card> Cards;

        public NewLobbyGameEvent(GameId gameId, EventVersion version, UserId adminId, NonEmptySet<Card> cards) : base(gameId, version)
        {
            AdminId = adminId;
            Cards = cards;
        }
    }

    public class AddPlayerGameEvent : GameEvent
    {
        public readonly UserId PlayerId;

        public AddPlayerGameEvent(GameId gameId, EventVersion version, UserId playerId) : base(gameId, version)
        {
            PlayerId = playerId;
        }
    }

    public class NewGameEvent : GameEvent
    {
        public NewGameEvent(GameId gameId, EventVersion version) : base(gameId, version)
        { }
    }

    public class NewRoundGameEvent : GameEvent
    {
        public readonly Round Round;

        public NewRoundGameEvent(GameId gameId, EventVersion version, Round round) : base(gameId, version)
        {
            Round = round;
        }
    }

    public class SelectCardGameEvent : GameEvent
    {
        public readonly PlayerCard PlayerCard;

        public SelectCardGameEvent(GameId gameId, EventVersion version, PlayerCard playerCard) : base(gameId, version)
        {
            PlayerCard = playerCard;
        }
    }

    public class EndRoundGameEvent : GameEvent
    {
        public readonly CardId ResultCardId;

        public EndRoundGameEvent(GameId gameId, EventVersion version, CardId resultCardId) : base(gameId, version)
        {
            ResultCardId = resultCardId;
        }
    }
}
