using GmailClient.Ui.Contracts;
using GmailClient.Ui.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GmailClient.Ui.Services
{
    public class GoogleOAuthDataService : IGoogleOAuthDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IAuthenticationDataService _authenticationDataService;

        public GoogleOAuthDataService(HttpClient httpClient, IAuthenticationDataService authenticationDataService)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _authenticationDataService = authenticationDataService;
        }

        public async Task<string> GetGoogleSignInUrlAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7075/api/googleoauth/generate-google-state");

            string accessToken = _authenticationDataService.GetAccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var json = JsonSerializer.Deserialize<GoogleUrlResponse>(jsonString, _jsonOptions);

                return json?.GoogleUrl;
            }

            return null;
        }
    }
}
