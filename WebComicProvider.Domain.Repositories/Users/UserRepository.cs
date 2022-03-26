using CodeCompanion.Extensions.Dapper.Postgres;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebComicProvider.Domain.Repositories.Interfaces.Users;
using WebComicProvider.Domain.Users;

namespace WebComicProvider.Domain.Repositories.Users
{
    public class UserRepository : SqlRepository<UserRepository>, IUserRepository
    {
        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger) : base(configuration, logger, nameof(UserRepository))
        {
        }

        public async Task<(UserModel?, IEnumerable<RoleModel>?)> Get(string username)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            var userAndRoles = await connection.QueryRefcursorsAsync(transaction, "get_user_with_roles_and_permissions", new { user_name = username });

            try
            {
                var user = await userAndRoles.ReadSingleOrDefaultAsync<UserModel>();
                if (user is not null)
                {
                    var roles = await userAndRoles.ReadAsync<RoleModel>();
                    return (user, roles);
                }
            }
            catch (NoRefcursorLeftException n)
            {
                Logger.LogError(n, "There was an error querying the database");
                throw;
            }
            Logger.LogInformation($"Unable to locate User {username}");
            return (null, null);
        }

        public async Task CreateUser(UserModel newUser, IEnumerable<RoleModel> roles)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            await connection.ExecuteAsync("call insert_new_user(@user_name, @display_name, @pass, @salt, @email, @iterations)", new
            {
                user_name = newUser.Username,
                display_name = newUser.Username,
                pass = newUser.Password,
                salt = newUser.Salt,
                email = newUser.Email,
                iterations = newUser.Iterations
            }, transaction);
            await transaction.CommitAsync();
        }

        public Task UpdateUser(UserModel user, IEnumerable<RoleModel> roles)
        {
            throw new NotImplementedException();
        }

    }
}
