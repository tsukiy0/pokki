using Core.UserDomain;
using System;
using System.Collections.Generic;

namespace Core.GameDomain
{
    public abstract class Event
    {
        public readonly GameId GameId;

        public Event(GameId gameId)
        {
            GameId = gameId;
        }

        public override bool Equals(object? obj)
        {
            return obj is Event @event &&
                   EqualityComparer<GameId>.Default.Equals(GameId, @event.GameId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GameId);
        }
    }

    public class NewGameEvent : Event
    {
        public readonly UserId AdminId;
        public readonly CardSet Cards;

        public NewGameEvent(GameId gameId, UserId adminId, CardSet cards) : base(gameId)
        {
            AdminId = adminId;
            Cards = cards;
        }
    }

    public class AddPlayerEvent : Event
    {
        public readonly UserId PlayerId;

        public AddPlayerEvent(GameId gameId, UserId playerId) : base(gameId)
        {
            PlayerId = playerId;
        }
    }

    public class NewRoundEvent : Event
    {
        public readonly RoundId RoundId;
        public readonly string RoundName;

        public NewRoundEvent(GameId gameId, RoundId roundId, string roundName) : base(gameId)
        {
            RoundId = roundId;
            RoundName = roundName;
        }
    }

    public class SelectCardEvent : Event
    {
        public readonly PlayerCard PlayerCard;

        public SelectCardEvent(GameId gameId, PlayerCard playerCard) : base(gameId)
        {
            PlayerCard = playerCard;
        }
    }

    public class EndRoundEvent : Event
    {
        public readonly CardId ResultCardId;

        public EndRoundEvent(GameId gameId, CardId resultCardId) : base(gameId)
        {
            ResultCardId = resultCardId;
        }
    }
}
