using GmailClient.Ui.Contracts;
using GmailClient.Ui.Helpers;
using GmailClient.Ui.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GmailClient.Ui.Services
{
    public class DraftDataService : IDraftDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IAuthenticationDataService _authenticationDataService;

        public DraftDataService(HttpClient httpClient, IAuthenticationDataService authenticationDataService)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _authenticationDataService = authenticationDataService;
        }

        public async Task<ApiResponse<bool>> CreateDraftAsync(CreateDraftRequest createDraftRequest)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7075/api/Draft")
                {
                    Content = new StringContent(JsonSerializer.Serialize(createDraftRequest), Encoding.UTF8, "application/json")
                };

                string accessToken = _authenticationDataService.GetAccessToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>(System.Net.HttpStatusCode.OK, true);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, errorMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> DeleteDraftAsync(string draftId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7075/api/Draft/{draftId}");

                string accessToken = _authenticationDataService.GetAccessToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>(System.Net.HttpStatusCode.OK, true);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, errorMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, ex.Message);
            }
        }

        public async Task<ApiResponse<DraftResponse>> GetDraftByIdAsync(string draftId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7075/api/Draft/{draftId}");

                string accessToken = _authenticationDataService.GetAccessToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var draft = JsonSerializer.Deserialize<DraftResponse>(content, _jsonOptions);
                    return new ApiResponse<DraftResponse>(System.Net.HttpStatusCode.OK, draft);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<DraftResponse>(System.Net.HttpStatusCode.BadRequest, null, errorMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<DraftResponse>(System.Net.HttpStatusCode.BadRequest, null, ex.Message);
            }
        }

        public async Task<ApiResponse<List<DraftResponse>>> GetDraftsAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7075/api/Draft");

                string accessToken = _authenticationDataService.GetAccessToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var drafts = JsonSerializer.Deserialize<List<DraftResponse>>(content, _jsonOptions);
                    return new ApiResponse<List<DraftResponse>>(System.Net.HttpStatusCode.OK, drafts);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<List<DraftResponse>>(System.Net.HttpStatusCode.BadRequest, null, errorMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<DraftResponse>>(System.Net.HttpStatusCode.BadRequest, null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateDraftAsync(UpdateDraftRequest updateDraftRequest)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:7075/api/Draft")
                {
                    Content = new StringContent(JsonSerializer.Serialize(updateDraftRequest), Encoding.UTF8, "application/json")
                };

                string accessToken = _authenticationDataService.GetAccessToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool>(System.Net.HttpStatusCode.OK, true);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, errorMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, ex.Message);
            }
        }
    }
}
