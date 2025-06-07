using GmailClient.Application.Features.Tokens.Commands.UpdateAccessToken;
using GmailClient.Application.Features.Tokens.Queries.GetAccessToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IMediator mediator) : Controller
    {
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
