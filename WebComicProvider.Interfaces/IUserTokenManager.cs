using WebComicProvider.Models.User;

namespace WebComicProvider.Interfaces
{
    public interface IUserTokenManager
    {
        Task CreateSession(string user, UserSession data);
        Task ExpireSession(string user);
        Task<UserSession?> GetSession(string user);
        Task RefreshToken(string user);
        Task UpdateSessionToken(string user, string newToken);
    }
}