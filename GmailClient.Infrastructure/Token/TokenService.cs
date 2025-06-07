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

        public async Task<string?> GetAccessTokenAsync(string refreshToken)
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
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            return data?["access_token"].GetString();
        }

    }
}
