using GmailClient.Application.DTOs;
using GmailClient.Application.Features.Account.Commands.Registration;
using GmailClient.Application.Features.Account.Queries.Authentication;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : Controller
    {

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var dtos = await mediator.Send(new AuthenticationQuery()
            {
                Email = request.Email,
                Password = request.Password
            });

            return Ok(dtos);
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> RegisterAsync(RegistrationRequest request)
        {
            var dtos = await mediator.Send(new RegistrationCommand()
            {
                UserName = request.UserName,
                Password = request.Password,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            });

            return Ok(dtos);
        }

        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Authentication");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return BadRequest();

            var accessToken = result.Properties.GetTokenValue("access_token");
            var refreshToken = result.Properties.GetTokenValue("refresh_token");
            var expiresAt = result.Properties.ExpiresUtc?.UtcDateTime ?? DateTime.UtcNow.AddHours(1);
            var userId = result.Principal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return BadRequest("User not found");

            var command = new SaveTokensCommand
            {
                AccessToken = accessToken ?? "",
                RefreshToken = refreshToken ?? "",
                ExpiresAt = expiresAt
            };

            var id = await mediator.Send(command);

            return Ok(new { TokenId = id, Message = "Token saved" });
        }
    }
}
