using Core.UserDomain;

namespace Core.GameDomain
{
    public struct PlayerCard
    {
        public readonly UserId PlayerId;
        public readonly CardId CardId;

        public PlayerCard(UserId userId, CardId cardId)
        {
            PlayerId = userId;
            CardId = cardId;
        }
    }
}
