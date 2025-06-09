using GmailClient.Application.Contracts;
using GmailClient.Application.DTOs;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using GmailClient.Application.Features.User.Commands.UpdateGoogleConnectionStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleOAuthController(
        IMediator mediator,
        IConfiguration configuration,
        IMemoryCache cache,
        ICurrentUserService currentUserService) : Controller
    {
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string code, [FromQuery] string state)
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"code", code},
                    {"client_id", configuration["Authentication:Google:ClientId"] },
                    {"client_secret", configuration["Authentication:Google:ClientSecret"] },
                    {"redirect_uri", "https://localhost:7075/api/googleoauth/google-callback"},
                    {"grant_type", "authorization_code"}
                })
            };

            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(tokenRequest);
            var json = await response.Content.ReadAsStringAsync();

            var tokenInfo = JsonSerializer.Deserialize<GoogleTokenResponse>(json);

            if (cache.TryGetValue(state, out string userId))
            {
                await mediator.Send(new SaveTokensCommand()
                {
                    AccessToken = tokenInfo.access_token,
                    ExpiresAt = tokenInfo.expires_in,
                    RefreshToken = tokenInfo.refresh_token,
                    UserId = userId
                });

                await mediator.Send(new UpdateGoogleConnectionStatusCommand
                {
                    IsConnected = true,
                    UserId = userId
                });
            }

            return Redirect(state);
        }

        [Authorize]
        [HttpPost("generate-google-state")]
        public IActionResult GenerateGoogleState()
        {
            var userId = currentUserService.UserId;

            var stateId = Guid.NewGuid().ToString("N");

            cache.Set(stateId, userId, TimeSpan.FromMinutes(10));

            var googleUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                            $"client_id={configuration["Authentication:Google:ClientId"]}&redirect_uri=https://localhost:7075/api/googleoauth/google-callback&response_type=code" +
                            $"&scope=openid email profile https://mail.google.com/&state={stateId}&access_type=offline&prompt=consent";

            return Ok(new { googleUrl });
        }
    }
}
