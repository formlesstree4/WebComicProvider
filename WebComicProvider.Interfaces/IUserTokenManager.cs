using WebComicProvider.Models.User;

namespace WebComicProvider.Interfaces
{
    public interface IUserTokenManager
    {
        /// <summary>
        /// Registers a <see cref="UserSession"/> and ties it to a User
        /// </summary>
        /// <param name="user">The username to create the session for</param>
        /// <param name="data">A reference to the <see cref="UserSession"/> for the user</param>
        /// <returns>A promise to create the session</returns>
        Task CreateSession(string user, UserSession data);

        /// <summary>
        /// Expires a session for the given User
        /// </summary>
        /// <param name="user">The username to expire a session for</param>
        /// <returns>A promise to expire the session if it exists</returns>
        Task ExpireSession(string user);

        /// <summary>
        /// Attempts to get the current <see cref="UserSession"/> for the supplied User
        /// </summary>
        /// <param name="user">The username to locate</param>
        /// <returns><see cref="UserSession"/></returns>
        Task<UserSession?> GetSession(string user);

        /// <summary>
        /// Refreshes the TTL for a current <see cref="UserSession"/>
        /// </summary>
        /// <param name="user">The username to extend the token for</param>
        /// <returns>A promise to refresh the TTL for the given User</returns>
        Task RefreshToken(string user);

        /// <summary>
        /// Modifies an existing User's token
        /// </summary>
        /// <param name="user">The username to look up and modify</param>
        /// <param name="newToken">The new token</param>
        /// <returns>A promise to update the User's token, provided they have a session still</returns>
        Task UpdateSessionToken(string user, string newToken);
    }
}