using GmailClient.Ui.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using GmailClient.Ui.Helpers;
using Microsoft.Extensions.Options;

namespace GmailClient.Ui.Services
{
    public class AuthenticationDataService : Contracts.IAuthenticationDataService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly string _baseUrl;

        public AuthenticationDataService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _baseUrl = apiSettings.Value.BaseUrl;
        }

        public async Task<ApiResponse<bool>> Login(LoginRequest request)
        {
            try
            {
                var authenticationRequest = new HttpRequestMessage(HttpMethod.Post, 
                    $"{_baseUrl}/api/Draft//api/authentication/authenticate")
                {
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(authenticationRequest);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var authenticationResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, _jsonOptions);


                    var jwtToken = authenticationResponse?.Token;

                    if (!string.IsNullOrEmpty(jwtToken))
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(jwtToken);

                        var claims = token.Claims.ToList();

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        await _httpContextAccessor.HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTime.UtcNow.AddDays(30)
                            });

                        _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", jwtToken, new CookieOptions
                        {
                            HttpOnly = false,
                            Secure = false,
                            SameSite = SameSiteMode.Lax,
                            Expires = DateTime.UtcNow.AddDays(30)
                        });

                        return new ApiResponse<bool>(System.Net.HttpStatusCode.OK, true);
                    }
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

        public string GetAccessToken()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
        }

        public async Task Logout()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                context.Response.Cookies.Delete("access_token");
            }
        }
        public async Task<ApiResponse<bool>> Register(RegistrationRequest request)
        {
            try
            {
                var registerRequest = new HttpRequestMessage(HttpMethod.Post,
                    $"{_baseUrl}/api/authentication/register")
                {
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(registerRequest);

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
