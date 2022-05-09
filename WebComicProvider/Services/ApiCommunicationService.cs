using System.Net.Http.Headers;
using System.Text;
using WebComicProvider.Models;
using WebComicProvider.Models.User;

namespace WebComicProvider.Services
{
    public sealed class ApiCommunicationService
    {
        private readonly HttpClient httpClient;

        public ApiCommunicationService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }







        public async Task<(bool, UserLoginResponse?)> PostAuthentication(UserLoginRequest request)
        {
            var content = request.ToJson();
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var loginResponse = await httpClient.PostAsync("api/User/Authenticate", bodyContent);
            if (!loginResponse.IsSuccessStatusCode) return (false, null);
            var result = (await loginResponse.Content.ReadAsStringAsync()).FromJson<UserLoginResponse>();
            return (true, result);
        }


        public async Task<(bool, UserRegistrationResult?)> PostRegistration(UserRegisterRequest request)
        {
            var content = request.ToJson();
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var loginResponse = await httpClient.PostAsync("api/User/Register", bodyContent);
            if (!loginResponse.IsSuccessStatusCode) return (false, null);
            var responsePayload = (await loginResponse.Content.ReadAsStringAsync()).FromJson<UserRegistrationResult>();
            return (responsePayload.Success, responsePayload);
        }

        public async Task PostLogout()
        {
            await httpClient.DeleteAsync("api/User/Logout");
        }


        public void SetBearerToken(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearBearerToken()
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
