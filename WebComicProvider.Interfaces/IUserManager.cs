using WebComicProvider.Models.User;

namespace WebComicProvider.Interfaces
{
    public interface IUserManager
    {
        Task<(bool, UserSession?)> Authenticate(UserLoginRequest request);
        Task<UserRegistrationResult> RegisterUser(UserRegisterRequest request);
    }
}
