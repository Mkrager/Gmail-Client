using GmailClient.Application.Contracts;
using GmailClient.Application.Features.Tokens.Commands.UpdateAccessToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IMediator mediator, ICurrentUserService currentUserService) : Controller
    {

        [HttpPatch(Name = "UpdateAccessToken")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> UpdateAccessToken()
        {
            var userId = currentUserService.UserId;
            await mediator.Send(new UpdateAccessTokenCommand() { UserId = userId });
            return NoContent();
        }
    }
}
