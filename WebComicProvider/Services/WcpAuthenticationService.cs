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
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public WcpAuthenticationService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this._authStateProvider = authStateProvider;
            this._localStorage = localStorage;
        }

        public async Task<bool> Authenticate(UserLoginRequest request)
        {
            var content = request.ToJson();
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var loginResponse = await _httpClient.PostAsync("api/User/Authenticate", bodyContent);

            if (!loginResponse.IsSuccessStatusCode)
                return false;

            var result = (await loginResponse.Content.ReadAsStringAsync()).FromJson<UserLoginResponse>();
            if (result is null)
            {
                return false;
            }

            await _localStorage.SetItemAsync("authToken", result.Token);
            ((WcpAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
            return true;

        }

        public async Task<bool> Register(UserRegisterRequest request)
        {
            var content = request.ToJson();
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var loginResponse = await _httpClient.PostAsync("api/User/Register", bodyContent);

            if (!loginResponse.IsSuccessStatusCode)
                return false;

            var responsePayload = (await loginResponse.Content.ReadAsStringAsync()).FromJson<UserRegistrationResult>();
            if (responsePayload.Success)
            {
                return await Authenticate(new UserLoginRequest { UserName = request.UserName, Password = request.Password });
            }
            return false;
        }

        public async Task Logout()
        {
            await _httpClient.DeleteAsync("api/User/Logout");
            await _localStorage.RemoveItemAsync("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            ((WcpAuthenticationStateProvider)_authStateProvider).NotifyUserLogout();
        }


    }
}
