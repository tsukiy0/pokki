using System;
using System.Collections.Generic;
using System.Linq;
using Core.UserDomain;

namespace Core.GameDomain
{
    public class PlayerRoleSet
    {
        public class NoAdminPlayerException : Exception { }
        public class MoreThanOneAdminException : Exception { }
        public class DuplicatePlayerException : Exception { }

        public readonly IList<PlayerRole> Value;

        public PlayerRoleSet(params PlayerRole[] value)
        {
            if (!value.Where(_ => _.Role == Role.Admin).Any())
            {
                throw new NoAdminPlayerException();
            }

            if (value.Where(_ => _.Role == Role.Admin).Count() > 1)
            {
                throw new MoreThanOneAdminException();
            }

            if (value.Select(_ => _.PlayerId).Distinct().Count() != value.Count())
            {
                throw new DuplicatePlayerException();
            }

            Value = value;
        }

        public UserId GetAdminId()
        {
            return Value.Where(_ => _.Role == Role.Admin).Single().PlayerId;
        }

        public bool HasPlayer(UserId playerId)
        {
            return Value.Where(_ => _.PlayerId.Equals(playerId)).Any();
        }
    }
}
