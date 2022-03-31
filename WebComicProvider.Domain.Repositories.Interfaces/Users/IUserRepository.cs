using WebComicProvider.Domain.Users;

namespace WebComicProvider.Domain.Repositories.Interfaces.Users
{
    public interface IUserRepository
    {
        Task CreateUser(UserModel newUser, IEnumerable<RoleModel> roles);
        Task<(UserModel?, IEnumerable<RoleModel>?)> Get(string username);
        Task UpdateUser(UserModel user, IEnumerable<RoleModel> roles);
        Task UpdateUserPassword(int userId, byte[] password, byte[] salt, int iterations);
    }
}
