using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text;
using WebComicProvider.Authentication;
using WebComicProvider.Models;
using WebComicProvider.Models.User;

namespace WebComicProvider.Services
{
    public sealed class WcpAuthenticationService
    {
        private readonly ApiCommunicationService apiService;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public WcpAuthenticationService(
            ApiCommunicationService apiService,
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            this.apiService = apiService;
            this._authStateProvider = authStateProvider;
            this._localStorage = localStorage;
        }

        public async Task<bool> Authenticate(UserLoginRequest request)
        {
            var (success, loginResponse) = await apiService.PostAuthentication(request);
            if (!success || loginResponse is null) return false;
            await _localStorage.SetItemAsync("authToken", loginResponse.Token);
            ((WcpAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(loginResponse.Token);
            apiService.SetBearerToken(loginResponse.Token);
            return true;

        }

        public async Task<(bool, string)> Register(UserRegisterRequest request)
        {
            var (success, registrationResponse) = await apiService.PostRegistration(request);
            if (!success) return (false, registrationResponse.ErrorMessage);
            var authentication = await Authenticate(new UserLoginRequest { UserName = request.UserName, Password = request.Password });
            return (authentication, authentication ? "" : "An issue occurred authenticating, please try again");
        }

        public async Task Logout()
        {
            await apiService.PostLogout();
            await _localStorage.RemoveItemAsync("authToken");
            apiService.ClearBearerToken();
            ((WcpAuthenticationStateProvider)_authStateProvider).NotifyUserLogout();
        }


    }
}
