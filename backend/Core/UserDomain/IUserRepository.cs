using System;
using System.Threading.Tasks;

namespace Core.UserDomain
{
    public class UserNotFoundException : Exception { }
    public interface IUserRepository
    {
        Task CreateUser(User user);
        Task<User> GetUser(UserId id);
    }
}
