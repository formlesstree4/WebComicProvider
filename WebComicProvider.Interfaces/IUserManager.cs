using WebComicProvider.Models.User;

namespace WebComicProvider.Interfaces
{
    public interface IUserManager
    {
        /// <summary>
        /// Performs an authentication request
        /// </summary>
        /// <param name="request"><see cref="UserLoginRequest"/></param>
        /// <returns>A tuple where the first Item indicates success and the second Item is the applicable <see cref="UserSession"/></returns>
        Task<(bool, UserSession?)> Authenticate(UserLoginRequest request);

        /// <summary>
        /// Registers a new User
        /// </summary>
        /// <param name="request"><see cref="UserRegisterRequest"/></param>
        /// <returns><see cref="UserRegistrationResult"/></returns>
        Task<UserRegistrationResult> RegisterUser(UserRegisterRequest request);

        /// <summary>
        /// Updates user details, such as Email Address and Display Names
        /// </summary>
        /// <param name="userId">The User ID to update</param>
        /// <param name="request"><see cref="UpdateUserProfileRequest"/></param>
        /// <returns>No result</returns>
        Task UpdateUser(int userId, UpdateUserProfileRequest request);

        /// <summary>
        /// Fetches the non-sensitive details of a User
        /// </summary>
        /// <param name="userId">The User ID</param>
        /// <returns><see cref="UserDetails"/></returns>
        Task<UserDetails?> Get(int userId);

        /// <summary>
        /// Fetches the non-sensitive details of a User
        /// </summary>
        /// <param name="username">The Username</param>
        /// <returns><see cref="UserDetails"/></returns>
        Task<UserDetails?> Get(string username);
    }
}
