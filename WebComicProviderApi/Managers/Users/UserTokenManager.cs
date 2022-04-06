using Microsoft.Extensions.Caching.Distributed;
using WebComicProvider.Interfaces;
using WebComicProvider.Models;
using WebComicProvider.Models.User;

namespace WebComicProviderApi.Managers.Users
{
    public sealed class UserTokenManager : IUserTokenManager
    {
        private readonly IDistributedCache cache;

        public UserTokenManager(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task RefreshToken(string user)
        {
            await cache.RefreshAsync(user);
        }

        public async Task CreateSession(string user, UserSession data)
        {
            await cache.SetStringAsync(user, data.ToJson(), new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });
        }

        public async Task UpdateSessionToken(string user, string newToken)
        {
            var currentSession = await GetSession(user);
            if (currentSession is not null)
            {
                currentSession.Token = newToken;
                await CreateSession(user, currentSession);
            }
        }

        public async Task ExpireSession(string user)
        {
            await cache.RemoveAsync(user);
        }

        public async Task<UserSession?> GetSession(string user)
        {
            var sessionData = await cache.GetStringAsync(user);
            return string.IsNullOrEmpty(sessionData) ? null : sessionData.FromJson<UserSession>();
        }


    }
}
