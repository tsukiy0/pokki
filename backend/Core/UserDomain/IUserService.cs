using System.Threading.Tasks;

namespace Core.UserDomain
{
    public interface IUserService
    {
        Task CreateUser(User user);
        Task<User> GetUser(UserId user);
    }
}
