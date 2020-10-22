using Core.UserDomain;

namespace Core.GameDomain
{
    public enum Role
    {
        Admin,
        Player
    }

    public struct PlayerRole
    {
        public readonly UserId PlayerId;
        public readonly Role Role;

        public PlayerRole(UserId playerId, Role role)
        {
            PlayerId = playerId;
            Role = role;
        }
    }
}