using System;

namespace Core.UserDomain
{
    public struct UserId
    {
        public readonly Guid Value;

        public UserId(Guid value)
        {
            Value = value;
        }
    }

    public struct User
    {
        public readonly UserId Id;
        public readonly string Name;

        public User(UserId id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
