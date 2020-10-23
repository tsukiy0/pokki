using System;
using Core.GameDomain;
using Core.UserDomain;
using Xunit;

namespace GameTests
{
    [Trait("Category", "Unit")]
    public class PlayerRoleSetTest
    {
        [Fact]
        public void ThrowWhenNoAdmin()
        {
            Assert.Throws<PlayerRoleSet.NoAdminPlayerException>(() => new PlayerRoleSet(
                new PlayerRole(
                    new UserId(Guid.NewGuid()),
                    Role.Player
                )
            ));
        }

        [Fact]
        public void ThrowWhenMoreThanOneAdmin()
        {
            Assert.Throws<PlayerRoleSet.MoreThanOneAdminException>(() => new PlayerRoleSet(
                new PlayerRole(
                    new UserId(Guid.NewGuid()),
                    Role.Admin
                ),
                new PlayerRole(
                    new UserId(Guid.NewGuid()),
                    Role.Admin
                )
            ));
        }

        [Fact]
        public void ThrowWhenDuplicatePlayer()
        {
            var playerId = new UserId(Guid.NewGuid());
            Assert.Throws<PlayerRoleSet.DuplicatePlayerException>(() => new PlayerRoleSet(
                new PlayerRole(
                    playerId,
                    Role.Admin
                ),
                new PlayerRole(
                    playerId,
                    Role.Player
                )
            ));
        }

        [Fact]
        public void GetAdminId()
        {
            var adminId = new UserId(Guid.NewGuid());
            var set = new PlayerRoleSet(
                new PlayerRole(
                    adminId,
                    Role.Admin
                ),
                new PlayerRole(
                    new UserId(Guid.NewGuid()),
                    Role.Player
                )
            );

            var actual = set.GetAdminId();

            Assert.Equal(adminId, actual);
        }

        [Fact]
        public void HasPlayer()
        {
            var adminId = new UserId(Guid.NewGuid());
            var set = new PlayerRoleSet(
                new PlayerRole(
                    adminId,
                    Role.Admin
                ),
                new PlayerRole(
                    new UserId(Guid.NewGuid()),
                    Role.Player
                )
            );

            var actual = set.HasPlayer(adminId);

            Assert.True(actual);
        }

        [Fact]
        public void AddPlayer()
        {
            var playerId = new UserId(Guid.NewGuid());
            var set = new PlayerRoleSet(
                new PlayerRole(
                    new UserId(Guid.NewGuid()),
                    Role.Admin
                )
            ).AddPlayer(playerId);

            var actual = set.HasPlayer(playerId);

            Assert.True(actual);
        }
    }
}
