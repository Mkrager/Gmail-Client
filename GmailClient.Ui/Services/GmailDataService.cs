using GmailClient.Ui.Contracts;
using GmailClient.Ui.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GmailClient.Ui.Services
{
    public class GmailDataService : IGmailDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IAuthenticationDataService _authenticationDataService;

        public GmailDataService(HttpClient httpClient, IAuthenticationDataService authenticationDataService)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _authenticationDataService = authenticationDataService;
        }

        public async Task<List<MessagesListVm>> GetAllMessages()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7075/api/Gmail");

            string accessToken = _authenticationDataService.GetAccessToken();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var messages = JsonSerializer.Deserialize<List<MessagesListVm>>(responseContent, _jsonOptions);

                return messages;
            }

            return new List<MessagesListVm>();
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7075/api/Gmail");

            string accessToken = _authenticationDataService.GetAccessToken();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
