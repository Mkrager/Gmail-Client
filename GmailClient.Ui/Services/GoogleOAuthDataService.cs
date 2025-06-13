using GmailClient.Ui.Contracts;
using GmailClient.Ui.Helpers;
using GmailClient.Ui.ViewModels;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GmailClient.Ui.Services
{
    public class GoogleOAuthDataService : IGoogleOAuthDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IAuthenticationDataService _authenticationDataService;
        private readonly string _baseUrl;
        public GoogleOAuthDataService(
            HttpClient httpClient,
            IAuthenticationDataService authenticationDataService,
            IOptions<ApiSettings> apiSettings)

        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _authenticationDataService = authenticationDataService;
            _baseUrl = apiSettings.Value.BaseUrl;
        }

        public async Task<ApiResponse<string>> GetGoogleSignInUrlAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"{_baseUrl}/api/googleoauth/generate-google-state");

                string accessToken = _authenticationDataService.GetAccessToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var url = JsonSerializer.Deserialize<GoogleUrlResponse>(jsonString, _jsonOptions);

                    return new ApiResponse<string>(System.Net.HttpStatusCode.OK, url?.GoogleUrl);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<string>(System.Net.HttpStatusCode.BadRequest, null, errorMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(System.Net.HttpStatusCode.BadRequest, null, ex.Message);
            }
        }
    }
}
