using GmailClient.Api.Services;
using GmailClient.Application.Contracts;
using GmailClient.Application.DTOs;
using GmailClient.Application.Features.GoogleOAuth.Commands.GoogleOAuthCallback;
using GmailClient.Application.Features.GoogleOAuth.Queries.GenerateGoogleOAuthUrl;
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
    public class GoogleOAuthController(IMediator mediator, ICurrentUserService currentUserService) : Controller
    {
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string code, [FromQuery] string state)
        {
            var result = await mediator.Send(new GoogleOAuthCallbackCommand()
            {
                Code = code,
                State = state
            });

            if (!result)
                return BadRequest("Invalid or expired state.");

            return Redirect(state);
        }

        [Authorize]
        [HttpPost("generate-google-state")]
        public async Task<IActionResult> GenerateGoogleState()
        {
            var userId = currentUserService.UserId;
            var googleUrl = await mediator.Send(new GenerateGoogleOAuthUrlQuery()
            {
                UserId = userId
            });
            return Ok(new { googleUrl });
        }
    }
}
