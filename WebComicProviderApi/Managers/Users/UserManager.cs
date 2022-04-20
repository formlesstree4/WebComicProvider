using System.Security.Cryptography;
using WebComicProvider.Domain.Repositories.Interfaces.Users;
using WebComicProvider.Domain.Users;
using WebComicProvider.Interfaces;
using WebComicProvider.Models.User;

namespace WebComicProviderApi.Managers.Users
{
    public sealed class UserManager : IUserManager
    {
        private const int SALT_LENGTH = 32;
        private const int PASSWORD_LENGTH = 256;
        private const int DEFAULT_ITERATIONS = 100_000;


        private readonly IUserRepository repository;

        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<(bool, UserSession?)> Authenticate(UserLoginRequest request)
        {
            (bool, UserSession?) badAuthResponse = (false, null);
            var userData = await repository.Get(request.UserName);
            if (userData is (null, _) || userData is (_, null))
            {
                return badAuthResponse;
            }
            if (!ValidatePassword(request.Password, userData.Item1.Password, userData.Item1.Salt, userData.Item1.Iterations))
            {
                return badAuthResponse;
            }

            // iterations check
            if (userData.Item1.Iterations != DEFAULT_ITERATIONS)
            {
                await RehashPassword(request, userData.Item1);
            }

            var userSession = new UserSession
            {
                Username = GetProperUsername(userData.Item1),
                Email = userData.Item1.EmailAddress,
                ImageUrl = userData.Item1.ProfileUrl,
                UserId = userData.Item1.ID,
                UserRoles = userData.Item2.Select(c => c.Name).ToList()
            };
            return (true, userSession);
        }

        public async Task<UserRegistrationResult> RegisterUser(UserRegisterRequest request)
        {
            var hashSet = HashPassword(request.Password);
            var userModel = new UserModel(0, request.UserName, request.UserName, hashSet.Password, hashSet.Salt, request.Email, DEFAULT_ITERATIONS, "");
            var roles = new List<RoleModel>();
            try
            {
                await repository.CreateUser(userModel, roles);
                return new UserRegistrationResult(true, "");
            }
            catch (Exception ex)
            {
                return new UserRegistrationResult(false, ex.Message);
            }
        }

        public async Task UpdateUser(int userId, UpdateUserProfileRequest request)
        {
            await repository.UpdateUser(new UserModel(
                userId,
                string.Empty,
                request.DisplayName,
                Array.Empty<byte>(),
                Array.Empty<byte>(),
                request.EmailAddress,
                -1,
                request.ProfileUrl), Array.Empty<RoleModel>());
        }

        public async Task<UserDetails?> Get(int userId)
        {
            var userData = await repository.Get(userId);
            if (userData is (null, _) || userData is (_, null))
            {
                return null;
            }
            return new UserDetails
            {
                Username = GetProperUsername(userData.Item1),
                Email = userData.Item1.EmailAddress,
                ImageUrl = userData.Item1.ProfileUrl,
                UserId = userData.Item1.ID,
                UserRoles = userData.Item2.Select(c => c.Name).ToList()
            };
        }

        public async Task<UserDetails?> Get(string username)
        {
            var userData = await repository.Get(username);
            if (userData is (null, _) || userData is (_, null))
            {
                return null;
            }
            return new UserDetails
            {
                Username = GetProperUsername(userData.Item1),
                Email = userData.Item1.EmailAddress,
                ImageUrl = userData.Item1.ProfileUrl,
                UserId = userData.Item1.ID,
                UserRoles = userData.Item2.Select(c => c.Name).ToList()
            };
        }


        private async Task RehashPassword(UserLoginRequest request, UserModel userModel)
        {
            var newSalt = CreatePasswordSalt();
            var newPasswordHashSet = HashPassword(request.Password, newSalt);
            await repository.UpdateUserPassword(userModel.ID, newPasswordHashSet.Password, newSalt, DEFAULT_ITERATIONS);
        }

        private static bool ValidatePassword(string passwordToValidate, byte[] targetPassword, byte[] salt, int iterations)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(passwordToValidate, salt, iterations);
            var result = pbkdf2.GetBytes(targetPassword.Length);
            return result.SequenceEqual(targetPassword);
        }

        private static PasswordHashSet HashPassword(string password, byte[]? existingSalt = null)
        {
            var salt = existingSalt ?? CreatePasswordSalt();
            var hasher = new Rfc2898DeriveBytes(password, salt, DEFAULT_ITERATIONS);
            var hash = hasher.GetBytes(PASSWORD_LENGTH);
            return new PasswordHashSet(salt, hash);
        }

        private static byte[] CreatePasswordSalt() => RandomNumberGenerator.GetBytes(SALT_LENGTH);

        private static string GetProperUsername(UserModel userModel)
        {
            return string.IsNullOrWhiteSpace(userModel.Display) ? userModel.Username : userModel.Display;
        }

    }

    internal record struct PasswordHashSet(byte[] Salt, byte[] Password);


}
