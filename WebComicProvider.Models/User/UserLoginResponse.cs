using System.Text.Json.Serialization;

namespace WebComicProvider.Models.User
{
    public sealed class UserLoginResponse
    {
        
        [JsonPropertyName("token")]
        public string Token { get; init; }

        public UserLoginResponse(string token)
        {
            Token = token;
        }
    }
}
