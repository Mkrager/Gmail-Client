using GmailClient.Application.Features.Tokens.Queries.GetAccessToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IMediator mediator) : Controller
    {
        [HttpGet("get-accesstoken", Name = "GetNewAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> GetNewAccessToken(string refreshToken)
        {
            var dtos = await mediator.Send(new GetAccessTokenQuery() { refreshToken = refreshToken });
            return Ok(dtos);
        }
    }
}
