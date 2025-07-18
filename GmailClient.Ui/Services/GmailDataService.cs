﻿using GmailClient.Ui.Contracts;
using GmailClient.Ui.Helpers;
using GmailClient.Ui.ViewModels;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GmailClient.Ui.Services
{
    public class GmailDataService : IGmailDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IAuthenticationDataService _authenticationDataService;
        private readonly string _baseUrl;

        public GmailDataService(
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

        public async Task<ApiResponse<MessagesListVm>> GetAllMessages(string pageToken = null)
        {
            try
            {
                var url = $"{_baseUrl}/api/Gmail";

                if (!string.IsNullOrEmpty(pageToken))
                {
                    url += $"?nextPageToken={Uri.EscapeDataString(pageToken)}";
                }

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                string accessToken = _authenticationDataService.GetAccessToken();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var messages = JsonSerializer.Deserialize<MessagesListVm>(responseContent, _jsonOptions);

                    return new ApiResponse<MessagesListVm>(System.Net.HttpStatusCode.OK, messages);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<MessagesListVm>(System.Net.HttpStatusCode.BadRequest, null, errorMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<MessagesListVm>(System.Net.HttpStatusCode.BadRequest, null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> SendEmailAsync(SendEmailRequest sendEmailRequest)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/api/Gmail")
                {
                    Content = new StringContent(JsonSerializer.Serialize(sendEmailRequest), Encoding.UTF8, "application/json")
                };

                string accessToken = _authenticationDataService.GetAccessToken();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _httpClient.SendAsync(request);

                string error = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var messages = JsonSerializer.Deserialize<MessagesListVm>(responseContent, _jsonOptions);

                    return new ApiResponse<bool>(System.Net.HttpStatusCode.OK, true);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonErrorHelper.GetErrorMessage(errorContent);
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, errorMessage);
            }
            catch(Exception ex)
            {
                return new ApiResponse<bool>(System.Net.HttpStatusCode.BadRequest, false, ex.Message);
            }
        }
    }
}
