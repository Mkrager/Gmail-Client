using GmailClient.Application.Features.Tokens.Commands.UpdateAccessToken;
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
    }
}
