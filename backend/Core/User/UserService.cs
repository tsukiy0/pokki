using System.Threading.Tasks;

namespace Core.User
{
    public interface IUserService
    {
        Task CreateUser(User user);
        Task<User> GetUser(UserId user);
    }
}
