using GmailClient.Application.Contracts;
using GmailClient.Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator, ICurrentUserService currentUserService) : Controller
    {
        [HttpGet(Name = "GetUserDetails")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UserDetailsVm>> GetUserDetails()
        {
            var id = currentUserService.UserId;
            var dtos = await mediator.Send(new GetUserDetailsQuery() { Id = id });
            return Ok(dtos);
        }
    }
}
