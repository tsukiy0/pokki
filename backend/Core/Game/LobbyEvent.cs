using Core.Shared;
using Core.User;

namespace Core.Game
{
    public abstract class LobbyEvent
    {
        public readonly GameId GameId;
        public readonly EventVersion Version;

        public LobbyEvent(GameId gameId, EventVersion version)
        {
            GameId = gameId;
            Version = version;
        }
    }

    public class NewLobbyEvent : LobbyEvent
    {
        public readonly UserId AdminId;
        public readonly NonEmptySet<Card> Cards;

        public NewLobbyEvent(GameId gameId, EventVersion version, UserId adminId, NonEmptySet<Card> cards) : base(gameId, version)
        {
            AdminId = adminId;
            Cards = cards;
        }
    }

    public class AddPlayerLobbyEvent : LobbyEvent
    {
        public readonly UserId PlayerId;

        public AddPlayerLobbyEvent(GameId gameId, EventVersion version, UserId playerId) : base(gameId, version)
        {
            PlayerId = playerId;
        }
    }

    public class NewGameLobbyEvent : LobbyEvent
    {
        public NewGameLobbyEvent(GameId gameId, EventVersion version) : base(gameId, version)
        { }
    }
}

