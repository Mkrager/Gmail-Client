using GmailClient.Ui.Contracts;
using GmailClient.Ui.Helpers;
using GmailClient.Ui.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GmailClient.Ui.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IAuthenticationDataService _authenticationDataService;

        public UserDataService(HttpClient httpClient, IAuthenticationDataService authenticationDataService)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _authenticationDataService = authenticationDataService;
        }

        public async Task<ApiResponse<UserDetailsResponse>> GetUserDetails()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7075/api/User/");

                string accessToken = _authenticationDataService.GetAccessToken();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var userDetails = JsonSerializer.Deserialize<UserDetailsResponse>(responseContent, _jsonOptions);

                    return new ApiResponse<UserDetailsResponse>(System.Net.HttpStatusCode.OK, userDetails);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<UserDetailsResponse>(System.Net.HttpStatusCode.BadRequest, null, errorMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDetailsResponse>(System.Net.HttpStatusCode.BadRequest, null, ex.Message);
            }
        }
    }
}
