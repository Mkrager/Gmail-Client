using GmailClient.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GmailClient.Infrastructure.Token
{
    public class TokenService : ITokenService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public TokenService(IConfiguration config)
        {
            _clientId = config["Authentication:Google:ClientId"]!;
            _clientSecret = config["Authentication:Google:ClientSecret"]!;
        }

        public async Task<(string? AccessToken, DateTime ExpiresAt)> GetAccessTokenAsync(string refreshToken)
        {
            using var client = new HttpClient();

            var values = new Dictionary<string, string>
                {
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "refresh_token", refreshToken },
                    { "grant_type", "refresh_token" }
                };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);

            if (!response.IsSuccessStatusCode)
                return (null, DateTime.MinValue);

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            if (data is null || !data.TryGetValue("access_token", out var accessTokenElement))
                return (null, DateTime.MinValue);

            var accessToken = accessTokenElement.GetString();

            DateTime expiresAt = DateTime.MinValue;
            if (data.TryGetValue("expires_in", out var expiresInElement))
            {
                var expiresInSeconds = expiresInElement.GetInt32();
                expiresAt = DateTime.UtcNow.AddSeconds(expiresInSeconds);
            }

            return (accessToken, expiresAt);
        }
    }
}
