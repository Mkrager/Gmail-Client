using Azure.Core;
using GmailClient.Application.DTOs;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using GmailClient.Application.Features.Tokens.Commands.UpdateAccessToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Text.Json;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IMediator mediator, IMemoryCache cache) : Controller
    {
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string code, [FromQuery] string state)
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"code", code},
                    {"client_id", "<removed-clientId>"},
                    {"client_secret", "<removed-secret>"},
                    {"redirect_uri", "https://localhost:7075/api/token/google-callback"},
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
            }

            return Redirect(state);
        }

        [Authorize]
        [HttpPost("generate-google-state")]
        public IActionResult GenerateGoogleState()
        {
            var userId = User.FindFirst("uid")?.Value;

            var stateId = Guid.NewGuid().ToString("N");

            cache.Set(stateId, userId, TimeSpan.FromMinutes(10));

            var googleUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                            $"client_id=<removed-clientId>&redirect_uri=https://localhost:7075/api/token/google-callback&response_type=code" +
                            $"&scope=openid email profile https://mail.google.com/&state={stateId}&access_type=offline&prompt=consent";

            return Ok(new { googleUrl });
        }

        [HttpPatch(Name = "UpdateAccessToken")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> UpdateAccessToken(string refreshToken, string userId)
        {
            var dtos = await mediator.Send(new UpdateAccessTokenCommand() { refreshToken = refreshToken });
            return Ok(dtos);
        }

        [HttpGet("check-user")]
        public IActionResult CheckUser()
        {
            var user = HttpContext.User;
            var isAuth = user.Identity?.IsAuthenticated ?? false;
            var uidClaim = user.FindFirst("uid")?.Value;

            return Ok(new
            {
                IsAuthenticated = isAuth,
                Uid = uidClaim
            });
        }
    }
}
