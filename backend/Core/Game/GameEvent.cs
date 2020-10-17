using System;
using System.Collections.Generic;
using Core.Shared;
using Core.User;

namespace Core.Game
{
    public abstract class GameEvent
    {
        public readonly GameId GameId;
        public readonly GameEventVersion Version;

        public GameEvent(GameId gameId, GameEventVersion version)
        {
            GameId = gameId;
            Version = version;
        }

        public override bool Equals(object? obj)
        {
            return obj is GameEvent @event &&
                   EqualityComparer<GameId>.Default.Equals(GameId, @event.GameId) &&
                   EqualityComparer<GameEventVersion>.Default.Equals(Version, @event.Version);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GameId, Version);
        }
    }

    public class NewGameEvent : GameEvent
    {
        public readonly UserId AdminId;
        public readonly NonEmptySet<Card> Cards;

        public NewGameEvent(GameId gameId, GameEventVersion version, UserId adminId, NonEmptySet<Card> cards) : base(gameId, version)
        {
            AdminId = adminId;
            Cards = cards;
        }
    }

    public class AddPlayerGameEvent : GameEvent
    {
        public readonly UserId PlayerId;

        public AddPlayerGameEvent(GameId gameId, GameEventVersion version, UserId playerId) : base(gameId, version)
        {
            PlayerId = playerId;
        }
    }

    public class StartGameEvent : GameEvent
    {
        public StartGameEvent(GameId gameId, GameEventVersion version) : base(gameId, version)
        { }
    }

    public class NewRoundGameEvent : GameEvent
    {
        public readonly Round Round;

        public NewRoundGameEvent(GameId gameId, GameEventVersion version, Round round) : base(gameId, version)
        {
            Round = round;
        }
    }

    public class SelectCardGameEvent : GameEvent
    {
        public readonly PlayerCard PlayerCard;

        public SelectCardGameEvent(GameId gameId, GameEventVersion version, PlayerCard playerCard) : base(gameId, version)
        {
            PlayerCard = playerCard;
        }
    }

    public class EndRoundGameEvent : GameEvent
    {
        public readonly CardId ResultCardId;

        public EndRoundGameEvent(GameId gameId, GameEventVersion version, CardId resultCardId) : base(gameId, version)
        {
            ResultCardId = resultCardId;
        }
    }
}
