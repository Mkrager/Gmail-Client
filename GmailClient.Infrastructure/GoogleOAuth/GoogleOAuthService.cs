using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.DTOs;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GmailClient.Infrastructure.GoogleOAuth
{
    public class GoogleOAuthService : IGoogleOAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoogleOAuthService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<GoogleTokenResponse> ExchangeCodeForTokensAsync(string code, string redirectUri)
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"code", code},
                    {"client_id", _configuration["Authentication:Google:ClientId"] },
                    {"client_secret", _configuration["Authentication:Google:ClientSecret"] },
                    {"redirect_uri", redirectUri},
                    {"grant_type", "authorization_code"}
                })
            };

            var response = await _httpClient.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenInfo = JsonSerializer.Deserialize<GoogleTokenResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return tokenInfo!;
        }
        public string GenerateGoogleAuthorizationUrl(string state)
        {
            var clientId = _configuration["Authentication:Google:ClientId"];
            var redirectUri = "https://localhost:7075/api/googleoauth/google-callback";
            var scope = "openid email profile https://mail.google.com/";
            var responseType = "code";
            var accessType = "offline";
            var prompt = "consent";

            return $"https://accounts.google.com/o/oauth2/v2/auth?" +
                   $"client_id={clientId}&redirect_uri={redirectUri}&response_type={responseType}" +
                   $"&scope={Uri.EscapeDataString(scope)}&state={state}&access_type={accessType}&prompt={prompt}";
        }
    }
}
