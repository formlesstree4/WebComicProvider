using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using WebComicProvider.Domain.Repositories.Interfaces.Users;
using WebComicProvider.Domain.Users;

namespace WebComicProvider.Domain.Repositories.Users
{
    public class UserRepository : SqlRepository<UserRepository>, IUserRepository
    {
        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger) : base(configuration, logger)
        {
        }

        public async Task<(UserModel?, IEnumerable<RoleModel>?)> Get(string username)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                using var userAndRoles = await connection.QueryMultipleAsync("spGetUserAndRoles", new { Username = username }, transaction, commandType: System.Data.CommandType.StoredProcedure);
                var user = await userAndRoles.ReadSingleOrDefaultAsync<UserModel>();
                if (user is not null)
                {
                    var roles = await userAndRoles.ReadAsync<RoleModel>();
                    return (user, roles);
                }
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex, "There was an error querying the database");
                throw;
            }
            return (null, null);
        }

        public async Task CreateUser(UserModel newUser, IEnumerable<RoleModel> roles)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            await connection.ExecuteAsync("spCreateUser", new
            {
                username = newUser.Username,
                displayName = newUser.Display,
                password = newUser.Password,
                salt = newUser.Salt,
                iterations = newUser.Iterations,
                email = newUser.EmailAddress,
                profileUrl = newUser.ProfileUrl,
                roles = roles.Select(r => r.ID).AsTableValuedParameter("dbo.TvpInt")
            }, transaction, commandType: System.Data.CommandType.StoredProcedure);
            await transaction.CommitAsync();
        }

        public async Task UpdateUser(UserModel user, IEnumerable<RoleModel> roles)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            await connection.ExecuteAsync("spUpdateUserDetails", new
            {
                userId = user.ID,
                displayName = user.Display,
                email = user.EmailAddress,
                profileUrl = user.ProfileUrl
            }, transaction, commandType: System.Data.CommandType.StoredProcedure);
            await transaction.CommitAsync();
        }

        public async Task UpdateUserPassword(int userId, byte[] password, byte[] salt, int iterations)
        {
            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            await connection.ExecuteAsync("spUpdateUserPassword", new
            {
                userId,
                password,
                salt,
                iterations
            }, transaction, commandType: System.Data.CommandType.StoredProcedure);
            await transaction.CommitAsync();
        }
    }
}
